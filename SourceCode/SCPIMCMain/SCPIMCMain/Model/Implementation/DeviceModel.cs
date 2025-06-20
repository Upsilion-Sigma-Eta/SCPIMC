using System.Threading;
using System.Threading.Tasks;
using SCPIMCMain.Common.Enum;
using SCPIMCMain.Model.Interface;

namespace SCPIMCMain.Model.Implementation
{
    public class DeviceModel : IDeviceModel, ISaveable, ILoadable
    {
        public string DeviceName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string DeviceType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string IPAddress { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Port { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EDeviceConnectionStatus ConnectionStatus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public EDeviceConnectionStatus Connect(string ipAddress, int port)
        {
            throw new NotImplementedException();
        }

        public EDeviceConnectionStatus Connect()
        {
            throw new NotImplementedException();
        }

        public Task<EDeviceConnectionStatus> ConnectAsync(string ipAddress, int port, CancellationToken cts)
        {
            throw new NotImplementedException();
        }

        public Task<EDeviceConnectionStatus> CoonectASync(CancellationToken cts)
        {
            throw new NotImplementedException();
        }

        public EDeviceConnectionStatus Disconnect()
        {
            throw new NotImplementedException();
        }

        public Task<EDeviceConnectionStatus> DisconnectAsync(CancellationToken cts)
        {
            throw new NotImplementedException();
        }

        public string Load(string filePath)
        {
            throw new NotImplementedException();
        }

        public void ReceiveCommand(uint timeout)
        {
            throw new NotImplementedException();
        }

        public Task<string> ReceiveCommandAsync(CancellationToken cts)
        {
            throw new NotImplementedException();
        }

        public void Save(string filePath, string jsonContent, bool isBinary = false)
        {
            throw new NotImplementedException();
        }

        public void SendCommand(string command, bool isQueryCommand)
        {
            throw new NotImplementedException();
        }

        public Task SendCommandAsync(string command, bool isQueryCommand, CancellationToken cts)
        {
            throw new NotImplementedException();
        }
    }
}