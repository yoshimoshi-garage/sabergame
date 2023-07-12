using Meadow.Hardware;

namespace SaberGame.Core;

public interface IInputService
{
    IDigitalInterruptPort LeftSaber { get; }
    IDigitalInterruptPort RightSaber { get; }
    IDigitalInterruptPort Reset { get; }
    IDigitalInterruptPort LeftScoreUp { get; }
    IDigitalInterruptPort LeftScoreDown { get; }
    IDigitalInterruptPort RightScoreUp { get; }
    IDigitalInterruptPort RightScoreDown { get; }
}
