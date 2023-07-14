using System.Threading.Tasks;

namespace SaberGame.Core;

public interface IAudioService
{
    Task Beep();
    bool Mute { get; set; }
}
