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
                LeftScoreUp = Device.Pins.D05.CreateDigitalInterruptPort(
                    InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                    TimeSpan.FromMilliseconds(50), TimeSpan.Zero),
                LeftScoreDown = Device.Pins.D07.CreateDigitalInterruptPort(
                    InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                    TimeSpan.FromMilliseconds(50), TimeSpan.Zero),
                RightScoreUp = Device.Pins.D08.CreateDigitalInterruptPort(
                    InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                    TimeSpan.FromMilliseconds(50), TimeSpan.Zero),
                RightScoreDown = Device.Pins.D12.CreateDigitalInterruptPort(
                    InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                    TimeSpan.FromMilliseconds(50), TimeSpan.Zero),
                Reset = Device.Pins.D13.CreateDigitalInterruptPort(
                    InterruptMode.EdgeFalling, ResistorMode.InternalPullUp,
                    TimeSpan.FromMilliseconds(50), TimeSpan.Zero),
                Display = new Max7219Display(
                    Device.CreateSpiBus(), Device.Pins.D15.CreateDigitalOutputPort())
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