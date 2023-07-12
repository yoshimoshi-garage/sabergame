using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Audio;
using Meadow.Hardware;
using SaberGame.Core;
using System;
using System.Threading.Tasks;

namespace SaberGame
{
    public class MeadowApp : App<F7FeatherV2>
    {
        private Game _game = default!;
        private const int ButtonDebounceDurationMs = 100;

        public override Task Initialize()
        {
            var config = new GameHardwareConfig
            {
                LeftSaber = Device.Pins.D00.CreateDigitalInterruptPort(
                    InterruptMode.EdgeRising, ResistorMode.InternalPullDown,
                    TimeSpan.FromMilliseconds(50), TimeSpan.Zero),
                RightSaber = Device.Pins.D01.CreateDigitalInterruptPort(
                    InterruptMode.EdgeRising, ResistorMode.InternalPullDown,
                    TimeSpan.FromMilliseconds(50), TimeSpan.Zero),
                LeftScoreUp = Device.Pins.D07.CreateDigitalInterruptPort(
                    InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                    TimeSpan.FromMilliseconds(ButtonDebounceDurationMs), TimeSpan.FromMilliseconds(ButtonDebounceDurationMs)),
                LeftScoreDown = Device.Pins.D05.CreateDigitalInterruptPort(
                    InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                    TimeSpan.FromMilliseconds(ButtonDebounceDurationMs), TimeSpan.FromMilliseconds(ButtonDebounceDurationMs)),
                RightScoreUp = Device.Pins.D12.CreateDigitalInterruptPort(
                    InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                    TimeSpan.FromMilliseconds(ButtonDebounceDurationMs), TimeSpan.FromMilliseconds(ButtonDebounceDurationMs)),
                RightScoreDown = Device.Pins.D08.CreateDigitalInterruptPort(
                    InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                    TimeSpan.FromMilliseconds(ButtonDebounceDurationMs), TimeSpan.FromMilliseconds(ButtonDebounceDurationMs)),
                Reset = Device.Pins.D13.CreateDigitalInterruptPort(
                    InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                    TimeSpan.FromMilliseconds(ButtonDebounceDurationMs), TimeSpan.FromMilliseconds(ButtonDebounceDurationMs)),
                Display = new Max7219Display(
                    Device.CreateSpiBus(), Device.Pins.D15.CreateDigitalOutputPort()),
                Piezo = new PiezoSpeaker(Device.Pins.D03)
                // Piezo on D06
            };

            _game = new Game(config);

            return base.Initialize();
        }

        public override Task Run()
        {
            _game.Start();

            return base.Run();
        }
    }
}