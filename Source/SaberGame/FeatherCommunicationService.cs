using Meadow;
using Meadow.Gateways;
using Meadow.Gateways.Bluetooth;
using SaberGame.Core;
using System;
using System.Threading;

namespace SaberGame
{
    internal class FeatherCommunicationService : ICommunicationService
    {
        private const string DeviceName = "SABERGAME"; // TODO: make this per-device configurable
        private const string ServiceName = "WiFiService";

        private Timer _bleShutdownTimer;
        private Timer? _wifiShutdownTimer;

        private static class Characteristics
        {
            public const string EnableWiFi = nameof(EnableWiFi);
            public const string SSID = nameof(SSID);
            public const string Passcode = nameof(Passcode);
            public const string Status = nameof(Status);
        }

        private ICharacteristic enable;
        private ICharacteristic ssid;
        private ICharacteristic passcode;
        private ICharacteristic status;

        public FeatherCommunicationService(IBluetoothAdapter ble)
        {
            // TODO: Start up BLE server for a few minutes to allow config
            Resolver.Log.Info($"Starting BLE Server...");
            _bleShutdownTimer = new Timer(ShutdownBluetooth, null, TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(-1));
            _bleShutdownTimer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(-1));

            ble.StartBluetoothServer(GetDefinition());

            status.SetValue("starting bluetooth");
            enable.ValueSet += OnEnableWiFiSet;
            ssid.ValueSet += OnSSIDSet;
            passcode.ValueSet += OnPasscodeSet;

            // TODO: are we configured for WiFi?

            // TODO: connect to WiFi
        }

        private void ShutdownBluetooth(object? state)
        {
            // TODO: enable BLE shutdown (needs OS work)
        }

        private void OnPasscodeSet(ICharacteristic c, object data)
        {
            Resolver.Log.Info($"Passcode has been set to {data}");
            _bleShutdownTimer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(-1));
        }

        private void OnSSIDSet(ICharacteristic c, object data)
        {
            Resolver.Log.Info($"SSID has been set to {data}");
            _bleShutdownTimer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(-1));
        }

        private void OnEnableWiFiSet(ICharacteristic c, object data)
        {
            Resolver.Log.Info($"WiFi Enabled has been set to {data}");
            _bleShutdownTimer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(-1));
        }

        private Definition GetDefinition()
        {
            return new Definition(
                DeviceName,
                new Service(
                    ServiceName,
                    42,
                    enable = new CharacteristicBool(
                        Characteristics.EnableWiFi,
                        uuid: "7d4d62de-6c3f-41ac-a165-769252dfbcb6",
                        permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                        properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                        ),
                    ssid = new CharacteristicString(
                        Characteristics.SSID,
                        uuid: "7d4d62de-6c3f-41ac-a165-769252dfbcb7",
                        maxLength: 20,
                        permissions: CharacteristicPermission.Write | CharacteristicPermission.Read,
                        properties: CharacteristicProperty.Write | CharacteristicProperty.Read
                        ),
                    passcode = new CharacteristicString(
                        Characteristics.Passcode,
                        uuid: "7d4d62de-6c3f-41ac-a165-769252dfbcb8",
                        maxLength: 20,
                        permissions: CharacteristicPermission.Write,
                        properties: CharacteristicProperty.Write
                        ),
                    status = new CharacteristicString(
                        Characteristics.Status,
                        uuid: "7d4d62de-6c3f-41ac-a165-769252dfbcb9",
                        maxLength: 20,
                        permissions: CharacteristicPermission.Read,
                        properties: CharacteristicProperty.Read
                        )
                    )
            );

        }
    }
}