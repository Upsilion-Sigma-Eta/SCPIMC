using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using System.Threading;
using System.Threading.Tasks;
using SCPIMCMain.Common.Enum;
using SCPIMCMain.Model.Interface;

namespace SCPIMCMain.Model.Implementation
{
    public class DeviceModel : IDeviceModel, ISaveable, ILoadable
    {
        private string _deviceName;
        private string _deviceType;
        private string _ipAddress;
        private int _port;
        private EDeviceConnectionStatus _connectionStatus;
        private TcpClient _tcpClient;

        public DeviceModel()
        {
            DeviceName = "Default";
            DeviceType = "Default";
            IPAddress = "0.0.0.0";
            Port = 0;

            _connectionStatus = EDeviceConnectionStatus.Disconnected;
            TcpClient = new TcpClient();
        }

        public TcpClient TcpClient
        {
            get
            {
                return _tcpClient;
            }
            protected set
            {
                if (_tcpClient != null && value == null)
                {
                    throw new Exception("Client must be non-null after initialized.");
                }
                else
                {
                    _tcpClient = value;
                }
            }
        }

        public string DeviceName
        {
            get => _deviceName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Device name cannot be null or empty.");
                }
                _deviceName = value;
            }
        }
        public string DeviceType
        {
            get => _deviceType;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Device Type cannot be null or empty.");
                }
                _deviceType = value;
            }
        }
        public string IPAddress
        {
            get => _ipAddress;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("IP Address cannot be null or empty.");
                }
                _ipAddress = value;
            }
        }
        public int Port
        {
            get => _port;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Port number cannot be below zero.");
                }
                _port = value;
            }
        }
        public EDeviceConnectionStatus ConnectionStatus
        {
            get
            {
                return _connectionStatus;
            }
            set
            {
                _connectionStatus = value;
            }
        }

        public EDeviceConnectionStatus Connect(string ipAddress, int port)
        {
            // Connect할 때 한 번만 IPAddress와 Port 값을 설정
            // 한 번 설정되고 나서는 변경할 수 없음.
            if (IPAddress == null)
            {
                IPAddress = ipAddress;
            }
            if (Port == 0)
            {
                Port = port;
            }

            return Connect();
        }

        public EDeviceConnectionStatus Connect()
        {
            try
            {
                if (_tcpClient == null)
                {
                    _tcpClient = new TcpClient(IPAddress, Port);

                    _connectionStatus = EDeviceConnectionStatus.Disconnected;

                    if (_tcpClient != null)
                    {
                        _connectionStatus = EDeviceConnectionStatus.Connecting;

                        _tcpClient.Connect(IPAddress, Port);

                        if (_tcpClient.Connected)
                        {
                            _connectionStatus = EDeviceConnectionStatus.Connected;

                            return _connectionStatus;
                        }
                        else
                        {
                            _connectionStatus = EDeviceConnectionStatus.Disconnected;

                            return _connectionStatus;
                        }
                    }
                }

                return _connectionStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not connect to host device. Reason: {ex.Message}");

                if (_connectionStatus == EDeviceConnectionStatus.Connecting)
                {
                    Disconnect();
                }

                return _connectionStatus;
            }
        }

        public async Task<EDeviceConnectionStatus> ConnectAsync(string ipAddress, int port, CancellationToken cts)
        {
            try
            {
                if (IPAddress == null)
                {
                    _ipAddress = ipAddress;
                }
                if (Port == 0)
                {
                    _port = port;
                }

                EDeviceConnectionStatus result = await ConnectAsync(cts);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not connect to host device. Reason: {ex.Message}");

                if (_connectionStatus == EDeviceConnectionStatus.Connecting)
                {
                    await DisconnectAsync(cts);
                }

                return _connectionStatus;
            }
        }

        public async Task<EDeviceConnectionStatus> ConnectAsync(CancellationToken cts)
        {
            try
            {
                if (_tcpClient == null)
                {
                    _tcpClient = new TcpClient(IPAddress, Port);

                    _connectionStatus = EDeviceConnectionStatus.Disconnected;

                    if (_tcpClient != null)
                    {
                        _connectionStatus = EDeviceConnectionStatus.Connecting;

                        await _tcpClient.ConnectAsync(IPAddress, Port);

                        if (_tcpClient.Connected)
                        {
                            _connectionStatus = EDeviceConnectionStatus.Connected;

                            return _connectionStatus;
                        }
                        else
                        {
                            _connectionStatus = EDeviceConnectionStatus.Disconnected;

                            return _connectionStatus;
                        }
                    }
                }

                return _connectionStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not connect to host device. Reason: {ex.Message}");

                if (_connectionStatus == EDeviceConnectionStatus.Connecting)
                {
                    await DisconnectAsync(cts);
                }

                return _connectionStatus;
            }
        }

        public EDeviceConnectionStatus Disconnect()
        {
            try
            {
                if (_tcpClient == null)
                {
                    throw new Exception($"No connection to the host");
                }

                _connectionStatus = EDeviceConnectionStatus.Disconnecting;

                _tcpClient.Dispose();
                _tcpClient = null;

                _connectionStatus = EDeviceConnectionStatus.Disconnected;

                return _connectionStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not disconnect from host device. Reason: {ex.Message}");

                return _connectionStatus;
            }
        }

        public async Task<EDeviceConnectionStatus> DisconnectAsync(CancellationToken cts)
        {
            try
            {
                return Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not disconnect from host device. Reason: {ex.Message}");

                return _connectionStatus;
            }
        }

        public string Load(string filePath)
        {
            return string.Empty;
        }

        public string ReceiveCommand(uint timeout)
        {
            try
            {
                if (_tcpClient == null || !_tcpClient.Connected)
                {
                    throw new Exception($"No connection made to host.");
                }

                if (_tcpClient.GetStream() is NetworkStream stream)
                {
                    if (stream.CanRead)
                    {
                        byte[] buffer = new byte[1024];
                        int readedCount = stream.Read(buffer, 0, 1024);

                        if (readedCount <= 0)
                        {
                            throw new Exception($"There is nothing to read.");
                        }

                        return ASCIIEncoding.ASCII.GetString(buffer, 0, readedCount);
                    }
                }

                return "";

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not receive anything from the host device. Reason: {ex.Message}");

                return "";
            }
        }

        public async Task<string> ReceiveCommandAsync(CancellationToken cts)
        {
            try
            {
                if (_tcpClient == null || !_tcpClient.Connected)
                {
                    throw new Exception($"No connection made to host.");
                }

                if (_tcpClient.GetStream() is NetworkStream stream)
                {
                    if (stream.CanRead)
                    {
                        byte[] buffer = new byte[1024];
                        int readedCount = stream.Read(buffer, 0, 1024);

                        if (readedCount <= 0)
                        {
                            throw new Exception($"There is nothing to read.");
                        }

                        return ASCIIEncoding.ASCII.GetString(buffer, 0, readedCount);
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not receive anything from the host device. Reason: {ex.Message}");

                return "";
            }
        }

        public void Save(string filePath, string jsonContent, bool isBinary = false)
        {

        }

        public void SendCommand(string command, bool isQueryCommand)
        {
            try
            {
                if (_tcpClient == null || _tcpClient.Connected == false)
                {
                    throw new Exception($"Not connected to host.");
                }

                if (_tcpClient.GetStream() is NetworkStream stream)
                {
                    stream.Write(UTF8Encoding.ASCII.GetBytes(command));
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't send a command {command} Reason: {ex.Message}");
            }
        }

        public async Task SendCommandAsync(string command, bool isQueryCommand, CancellationToken cts)
        {
            try
            {
                if (_tcpClient == null || _tcpClient.Connected == false)
                {
                    throw new Exception($"Not connected to host.");
                }

                if (_tcpClient.GetStream() is NetworkStream stream)
                {
                    stream.WriteAsync(UTF8Encoding.ASCII.GetBytes(command));
                    stream.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't send a command {command} Reason: {ex.Message}");
            }
        }
    }
}