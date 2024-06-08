using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeatherStation
{
    public interface IBluetoothService
    {
        Task ConnectAsync(string deviceName);
        Task<string> ReadDataAsync();
    }
}
