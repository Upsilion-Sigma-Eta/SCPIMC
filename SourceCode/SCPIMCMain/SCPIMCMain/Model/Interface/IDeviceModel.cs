using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCPIMCMain.Common.Enum;

namespace SCPIMCMain.Model.Interface
{
    public interface IDeviceModel
    {
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }

        public EDeviceConnectionStatus ConnectionStatus { get; set; }
        public EDeviceConnectionStatus Connect(string ipAddress, int port);
        public EDeviceConnectionStatus Connect();
        public EDeviceConnectionStatus Disconnect();

        public Task<EDeviceConnectionStatus> ConnectAsync(string ipAddress, int port, CancellationToken cts);
        public Task<EDeviceConnectionStatus> CoonectASync(CancellationToken cts);
        public Task<EDeviceConnectionStatus> DisconnectAsync(CancellationToken cts);

        public Task SendCommandAsync(string command, bool isQueryCommand, CancellationToken cts);
        public Task<string> ReceiveCommandAsync(CancellationToken cts);

        public void SendCommand(string command, bool isQueryCommand);
        public void ReceiveCommand(uint timeout);
    }
}
