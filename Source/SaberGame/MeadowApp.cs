using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using SaberGame.Core;
using System;
using System.Threading.Tasks;

namespace SaberGame
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private Game _game = default!;
        private const int TouchDebounceDurationMs = 50;
        private const int ButtonDebounceDurationMs = 100;
        private const int GlitchFilterDurationms = 1;

        public override Task Initialize()
        {
            Resolver.Services.Add<IInputService>(
                new InputService
                {
                    LeftSaber = Device.Pins.D00.CreateDigitalInterruptPort(
                        InterruptMode.EdgeRising, ResistorMode.InternalPullDown,
                        TimeSpan.FromMilliseconds(TouchDebounceDurationMs),
                        TimeSpan.FromMilliseconds(GlitchFilterDurationms)),
                    RightSaber = Device.Pins.D01.CreateDigitalInterruptPort(
                        InterruptMode.EdgeRising, ResistorMode.InternalPullDown,
                        TimeSpan.FromMilliseconds(TouchDebounceDurationMs),
                        TimeSpan.FromMilliseconds(GlitchFilterDurationms)),
                    LeftScoreUp = Device.Pins.D07.CreateDigitalInterruptPort(
                        InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                        TimeSpan.FromMilliseconds(ButtonDebounceDurationMs),
                        TimeSpan.FromMilliseconds(ButtonDebounceDurationMs)),
                    LeftScoreDown = Device.Pins.D05.CreateDigitalInterruptPort(
                        InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                        TimeSpan.FromMilliseconds(ButtonDebounceDurationMs),
                        TimeSpan.FromMilliseconds(ButtonDebounceDurationMs)),
                    RightScoreUp = Device.Pins.D12.CreateDigitalInterruptPort(
                        InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                        TimeSpan.FromMilliseconds(ButtonDebounceDurationMs),
                        TimeSpan.FromMilliseconds(ButtonDebounceDurationMs)),
                    RightScoreDown = Device.Pins.D08.CreateDigitalInterruptPort(
                        InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                        TimeSpan.FromMilliseconds(ButtonDebounceDurationMs),
                        TimeSpan.FromMilliseconds(ButtonDebounceDurationMs)),
                    Reset = Device.Pins.D13.CreateDigitalInterruptPort(
                        InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                        TimeSpan.FromMilliseconds(ButtonDebounceDurationMs),
                        TimeSpan.FromMilliseconds(ButtonDebounceDurationMs))
                });

            Resolver.Services.Add<IDisplayService>(
                new Max7219DisplayService(
                    Device.CreateSpiBus(),
                    Device.Pins.D15.CreateDigitalOutputPort()));

            Resolver.Services.Add<IAudioService>(
                new FeatherAudioService(
                    Device.Pins.D03));

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