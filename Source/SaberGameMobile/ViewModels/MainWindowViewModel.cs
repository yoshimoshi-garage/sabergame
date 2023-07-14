using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace SaberGameMobile.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private BluetoothService _bluetooth;

    public ReactiveCommand<Unit, Unit> BleScanCommand { get; set; }

    public MainWindowViewModel()
    {
        _bluetooth = Locator.Current.GetService<BluetoothService>() ?? throw new Exception("No bluetooth service");

        BleScanCommand = ReactiveCommand.CreateFromTask(BleScan);
    }

    private async Task BleScan()
    {
        await _bluetooth.Scan();
    }
}
