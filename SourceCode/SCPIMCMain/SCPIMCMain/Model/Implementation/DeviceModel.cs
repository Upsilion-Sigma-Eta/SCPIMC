using System.Net.Sockets;
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
                if (value <= 0)
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