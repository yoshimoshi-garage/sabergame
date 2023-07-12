using Meadow.Foundation.Audio;
using Meadow.Hardware;

namespace SaberGame.Core;

public class GameHardwareConfig
{
    public IDigitalInterruptPort LeftSaber { get; set; }
    public IDigitalInterruptPort RightSaber { get; set; }
    public IDigitalInterruptPort Reset { get; set; }
    public IDigitalInterruptPort LeftScoreUp { get; set; }
    public IDigitalInterruptPort LeftScoreDown { get; set; }
    public IDigitalInterruptPort RightScoreUp { get; set; }
    public IDigitalInterruptPort RightScoreDown { get; set; }
    public IGameDisplay Display { get; set; }
    public PiezoSpeaker? Piezo { get; set; }
}
