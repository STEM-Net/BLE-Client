using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace StemNetBluetoothClientApp.BLE
{
    class BLEReader
    {
        public static async Task<string> Read()
        {
            StringBuilder sb = new StringBuilder();

            // Replace with your characteristic UUID
            Guid characteristicUuid = new Guid("ca73b3ba-39f6-4ab3-91ae-186dc9577d99");
            try
            {
                var adapter = CrossBluetoothLE.Current.Adapter;
                IDevice device = null;
                adapter.DeviceDiscovered += (sender, args) =>
                {
                    if (args.Device.Name == "BME280_ESP32")
                    {
                        device = args.Device;
                        adapter.StopScanningForDevicesAsync();
                    }
                };
                await adapter.StartScanningForDevicesAsync();
                if (device != null)
                {
                    await adapter.ConnectToDeviceAsync(device);
                    var service = await device.GetServiceAsync(characteristicUuid);
                    var characteristic = await service.GetCharacteristicAsync(characteristicUuid);
                    await characteristic.StartUpdatesAsync();
                    characteristic.ValueUpdated += (sender, args) =>
                    {
                        byte[] data = args.Characteristic.Value;
                        sb.Append($"Received data: {BitConverter.ToString(data)}");
                    };
                }
                else
                {
                    sb.Append("Device 'BME280_ESP32' not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return sb.ToString();
        }
    }
}