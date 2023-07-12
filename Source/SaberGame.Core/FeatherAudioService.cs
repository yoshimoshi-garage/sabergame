using Meadow.Foundation.Audio;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace SaberGame.Core;

public class FeatherAudioService : IAudioService
{
    private PiezoSpeaker _piezo;
    private Frequency _beepLow = new Frequency(4000);
    private Frequency _beepHigh = new Frequency(4500);

    public FeatherAudioService(IPin piezoPin)
    {
        _piezo = new PiezoSpeaker(piezoPin);
    }

    public async Task Beep()
    {
        var duration = TimeSpan.FromMilliseconds(20);
        for (var i = 0; i < 8; i++)
        {
            await _piezo.PlayTone(_beepLow, duration);
            await _piezo.PlayTone(_beepHigh, duration);
        }
    }
}
