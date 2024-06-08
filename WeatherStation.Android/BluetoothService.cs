using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Bluetooth;
using Java.Util;
using Xamarin.Forms;
using WeatherStation.Droid;

[assembly: Dependency(typeof(BluetoothService))]
namespace WeatherStation.Droid
{
    public class BluetoothService : IBluetoothService
    {
        private BluetoothAdapter _bluetoothAdapter;
        private BluetoothDevice _device;
        private BluetoothSocket _socket;
        private System.IO.Stream _inputStream;

        public BluetoothService()
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        }

        public async Task ConnectAsync(string deviceName)
        {
            if (_bluetoothAdapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (!_bluetoothAdapter.IsEnabled)
                throw new Exception("Bluetooth is not enabled.");

            _device = _bluetoothAdapter.BondedDevices.FirstOrDefault(d => d.Name == deviceName);

            if (_device == null)
                throw new Exception("Named device not found.");

            try
            {
                // UUID for Serial Port Profile (SPP)
                _socket = _device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805F9B34FB"));
                await _socket.ConnectAsync();

                _inputStream = _socket.InputStream;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to connect to device.", ex);
            }
        }

        public async Task<string> ReadDataAsync()
        {
            if (_inputStream == null)
                throw new Exception("Not connected to a device.");

            try
            {
                byte[] buffer = new byte[1024];
                int bytes = await _inputStream.ReadAsync(buffer, 0, buffer.Length);

                // Assuming the ESP32 sends the integer as a string
                return Encoding.UTF8.GetString(buffer, 0, bytes).Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to read data from device.", ex);
            }
        }


    }
}