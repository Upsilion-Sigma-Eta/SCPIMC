using SCPIMCMain.Common.Enum;

namespace SCPIMCMain.Model.Interface
{
    public interface IDeviceModel
    {
        public string DeviceName { get; set; }
        public string DeviceType { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }

        public EDeviceConnectionStatus ConnectionStatus { get; set; }
        public EDeviceConnectionStatus Func_Connect(string __ipAddress, int __port);
        public EDeviceConnectionStatus Func_Connect();
        public EDeviceConnectionStatus Func_Disconnect();

        public Task<EDeviceConnectionStatus> Func_ConnectAsync(string __ipAddress, int __port, CancellationToken __cts);
        public Task<EDeviceConnectionStatus> Func_ConnectAsync(CancellationToken __cts);
        public Task<EDeviceConnectionStatus> Func_DisconnectAsync(CancellationToken __cts);

        public Task Func_SendCommandAsync(string __command, bool __isQueryCommand, CancellationToken __cts);
        public Task<string> Func_ReceiveCommandAsync(CancellationToken __cts);

        public void Func_SendCommand(string __command, bool __isQueryCommand);
        public string Func_ReceiveCommand(uint __timeout);
    }
}
