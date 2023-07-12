using Meadow;
using Meadow.Hardware;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SaberGame.Core;

public class Game
{
    private GameState _state = GameState.ScoreUpdate;
    private int _scoreLeft = 0;
    private int _scoreRight = 0;
    private TouchState _currentTouch;
    private Thread? _gameLoop = null;

    private IAudioService? _audioService;
    private IDisplayService? _displayService;
    private IInputService? _inputService;

    private const int GameLoopPeriodMs = 200;

    public TimeSpan PostTouchDelay { get; set; } = TimeSpan.FromSeconds(3);

    public Game()
    {
        _audioService = Resolver.Services.Get<IAudioService>();
        _displayService = Resolver.Services.Get<IDisplayService>();
        _inputService = Resolver.Services.Get<IInputService>();

        if (!ValidateHardwareSetup())
        {
            Resolver.Log.Error("INCORRECT HARDWARE SETUP");
            return;
        }

        if (_inputService != null) // this gets checked in the validation above
        {
            _inputService.LeftSaber.Changed += OnLeftSaberContact;
            _inputService.RightSaber.Changed += OnRightSaberContact;
            _inputService.Reset.Changed += OnReset;
            _inputService.LeftScoreUp.Changed += OnLeftScoreUp;
            _inputService.LeftScoreDown.Changed += OnLeftScoreDown;
            _inputService.RightScoreUp.Changed += OnRightScoreUp;
            _inputService.RightScoreDown.Changed += OnRightScoreDown;
        }
    }

    public void Start()
    {
        if (_gameLoop == null)
        {
            _audioService?.Beep(); // beep once now to force interp to build the audio pipeline

            _gameLoop = new Thread(GameLoop);
            _gameLoop.Start();
        }
    }

    private void OnLeftScoreUp(object sender, DigitalPortResult e)
    {
        if (_state == GameState.WaitingForTouch)
        {
            _scoreLeft++;
            _state = GameState.ScoreUpdate;
        }
    }

    private void OnLeftScoreDown(object sender, DigitalPortResult e)
    {
        if (_state == GameState.WaitingForTouch)
        {
            _scoreLeft--;
            _state = GameState.ScoreUpdate;
        }
    }

    private void OnRightScoreUp(object sender, DigitalPortResult e)
    {
        if (_state == GameState.WaitingForTouch)
        {
            _scoreRight++;
            _state = GameState.ScoreUpdate;
        }
    }

    private void OnRightScoreDown(object sender, DigitalPortResult e)
    {
        if (_state == GameState.WaitingForTouch)
        {
            _scoreRight--;
            _state = GameState.ScoreUpdate;
        }
    }

    private void OnReset(object sender, DigitalPortResult e)
    {
        if (_state == GameState.WaitingForTouch)
        {
            _scoreLeft = _scoreRight = 0;
            _state = GameState.ScoreUpdate;
        }
    }

    private void OnLeftSaberContact(object sender, DigitalPortResult e)
    {
        if (_state == GameState.WaitingForTouch)
        {
            _scoreLeft++;
            _currentTouch = TouchState.Left;
            _state = GameState.Touched;
            _ = _audioService?.Beep();

            Resolver.Log.Info("TOUCH LEFT");
        }
    }

    private void OnRightSaberContact(object sender, DigitalPortResult e)
    {
        if (_state == GameState.WaitingForTouch)
        {
            _scoreRight++;
            _currentTouch = TouchState.Right;
            _state = GameState.Touched;
            _ = _audioService?.Beep();

            Resolver.Log.Info("TOUCH RIGHT");
        }
    }

    private bool ValidateHardwareSetup()
    {
        if (_inputService == null)
        {
            Resolver.Log.Error("No InputService!");
            return false;
        }

        if (_inputService.LeftSaber.InterruptMode == InterruptMode.None)
        {
            Resolver.Log.Error("Left saber is not set for interrupts");
            return false;
        }
        if (_inputService.RightSaber.InterruptMode == InterruptMode.None)
        {
            Resolver.Log.Error("Right saber is not set for interrupts");
            return false;
        }

        return true;
    }

    private void GameLoop()
    {
        GameState? lastState = null;

        while (true)
        {
            if (lastState == null || lastState != _state)
            {
                Resolver.Log.Info($"GameState changed to {_state}");
                lastState = _state;
            }

            switch (_state)
            {
                case GameState.ScoreUpdate:
                    _displayService?.ShowScore(_scoreLeft, _scoreRight);
                    _state = GameState.WaitingForTouch;
                    break;
                case GameState.WaitingForTouch:
                    // nothing to do but wait
                    break;
                case GameState.Touched:
                    // update display with the current touch
                    switch (_currentTouch)
                    {
                        case TouchState.Right:
                            _displayService?.ShowTouchRight();
                            break;
                        case TouchState.Left:
                            _displayService?.ShowTouchLeft();
                            break;
                    }

                    // wait a while, then show scores again
                    _state = GameState.PostTouch;
                    Task.Run(async () =>
                    {
                        await Task.Delay(PostTouchDelay);
                        _state = GameState.ScoreUpdate;
                    });
                    break;
                case GameState.PostTouch:
                    // nothing to do - waiting on the post-touch timer to elapse
                    break;
            }

            Thread.Sleep(GameLoopPeriodMs);
        }
    }
}
