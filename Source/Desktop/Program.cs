﻿using Meadow;
using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Hardware;
using SaberGame.Core;

namespace SaberGame_Desktop
{
    public class MeadowApp : App<Windows>
    {
        private Ft232h _expander = new Ft232h();
        private Keyboard _keyboard = default!;
        private Game _game = default!;

        public bool UseConsole = true;

        public static async Task Main(string[] args)
        {
            await MeadowOS.Start(args);
        }

        public override Task Initialize()
        {
            _keyboard = new Keyboard();

            Resolver.Services.Add<IInputService>(
                new InputService
                {
                    LeftSaber = _keyboard.Pins.Left.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
                    RightSaber = _keyboard.Pins.Right.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
                    LeftScoreUp = _keyboard.Pins.Q.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
                    LeftScoreDown = _keyboard.Pins.A.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
                    RightScoreUp = _keyboard.Pins.W.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
                    RightScoreDown = _keyboard.Pins.S.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
                    Reset = _keyboard.Pins.D.CreateDigitalInterruptPort(InterruptMode.EdgeRising),
                });

            if (UseConsole)
            {
                Resolver.Services.Add<IDisplayService>(
                    new ConsoleDisplayService());
            }
            else
            {
                Resolver.Services.Add<IDisplayService>(
                    new Max7219DisplayService(
                        _expander.CreateSpiBus(),
                        _expander.Pins.C0.CreateDigitalOutputPort())
                        );
            }

            _game = new Game();

            return base.Initialize();
        }

        public override Task Run()
        {
            _game.Start();

            return base.Run();
        }
    }
}