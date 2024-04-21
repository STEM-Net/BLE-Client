using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace StemNetBluetoothClientApp.Model.BLE
{
    class BLEReader
    {
        public static async Task<string> Read()
        {
            StringBuilder sb = new StringBuilder();

            // Replace with your characteristic UUID
            Guid characteristicUuid = new Guid("ca73b3ba-39f6-4ab3-91ae-186dc9577d99");
            Guid serviceUuid = new Guid("91bad492-b950-4226-aa2b-4ede9fa42f59");
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
                    var service = await device.GetServiceAsync(serviceUuid);
                    if (service == null)
                    {
                        sb.AppendLine("Service '91bad492-b950-4226-aa2b-4ede9fa42f59' not found.");
                        return sb.ToString();
                    }
                    var characteristic = await service.GetCharacteristicAsync(characteristicUuid);
                    characteristic.ValueUpdated += (sender, args) =>
                    {
                        byte[] data = args.Characteristic.Value;
                        sb.AppendLine($"Received data: {BitConverter.ToString(data)}");
                    };
                    await characteristic.StartUpdatesAsync();
                    await Task.Delay(10000);
                }
                else
                {
                    sb.AppendLine("Device 'BME280_ESP32' not found.");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Error: {ex.Message} {ex.StackTrace}");
            }

            return sb.ToString();
        }
    }
}