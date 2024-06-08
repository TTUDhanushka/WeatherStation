using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace WeatherStation
{
    public partial class MainPage : ContentPage
    {
        private IBluetoothService _bluetoothService;

        public MainPage()
        {
            InitializeComponent();
            _bluetoothService = DependencyService.Get<IBluetoothService>();

            if (_bluetoothService == null)
            {
                DisplayAlert("Error", "Bluetooth service not found", "OK");
            }
        }

        private async void OnConnectClicked(object sender, EventArgs e)
        {
            try
            {
                if (_bluetoothService == null)
                {
                    await DisplayAlert("Error", "Bluetooth service not available", "OK");
                    return;
                }

                string deviceName = DeviceNameEntry.Text;
                if (string.IsNullOrWhiteSpace(deviceName))
                {
                    await DisplayAlert("Error", "Please enter a device name", "OK");
                    return;
                }

                await _bluetoothService.ConnectAsync(deviceName);
                StartReadingData();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task ReadBluetoothDataAsync()
        {
            try
            {
                string data = await _bluetoothService.ReadDataAsync();
                Device.BeginInvokeOnMainThread(() =>
                {
                    DataLabel.Text = data;
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Error", ex.Message, "OK");
                });
            }
        }

        private void StartReadingData()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                // Call the async method, but don't wait for it
                _ = ReadBluetoothDataAsync();
                return true; // True to keep the timer running
            });
        }
    }
}
