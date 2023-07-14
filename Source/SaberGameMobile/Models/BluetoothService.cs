using InTheHand.Bluetooth;
using System;
using System.Threading.Tasks;

namespace SaberGameMobile;

internal class BluetoothService
{
    public async Task Scan()
    {
        Bluetooth.AdvertisementReceived += OnAdvertisementReceived;
        var scan = await Bluetooth.RequestLEScanAsync();
    }

    private void OnAdvertisementReceived(object? sender, BluetoothAdvertisingEvent e)
    {
        throw new NotImplementedException();
    }
}
