using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SCPIMCMain.Common.Enum;
using SCPIMCMain.Model.Interface;

namespace SCPIMCMain.Model.Implementation
{
    public class DeviceModel : IDeviceModel, ISaveable, ILoadable
    {
        private string _device_name;
        private string _device_type;
        private string _ip_address;
        private int _port;
        private EDeviceConnectionStatus _connection_status;
        private TcpClient _tcp_client;

        public DeviceModel()
        {
            DeviceName = "Default";
            DeviceType = "Default";
            IpAddress = "0.0.0.0";
            Port = 0;

            _connection_status = EDeviceConnectionStatus.Disconnected;
            TcpClient = new TcpClient();
        }

        public TcpClient TcpClient
        {
            get
            {
                return _tcp_client;
            }
            protected set
            {
                if (_tcp_client != null && value == null)
                {
                    throw new Exception("Client must be non-null after initialized.");
                }
                else
                {
                    _tcp_client = value;
                }
            }
        }

        public string DeviceName
        {
            get => _device_name;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Device name cannot be null or empty.");
                }
                _device_name = value;
            }
        }
        public string DeviceType
        {
            get => _device_type;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Device Type cannot be null or empty.");
                }
                _device_type = value;
            }
        }
        public string IpAddress
        {
            get => _ip_address;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("IP Address cannot be null or empty.");
                }
                _ip_address = value;
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
                return _connection_status;
            }
            set
            {
                _connection_status = value;
            }
        }

        public EDeviceConnectionStatus Func_Connect(string __ipAddress, int __port)
        {
            // Connect할 때 한 번만 IPAddress와 Port 값을 설정
            // 한 번 설정되고 나서는 변경할 수 없음.
            if (IpAddress == null)
            {
                IpAddress = __ipAddress;
            }
            if (Port == 0)
            {
                Port = __port;
            }

            return Func_Connect();
        }

        public EDeviceConnectionStatus Func_Connect()
        {
            try
            {
                if (_tcp_client == null)
                {
                    _tcp_client = new TcpClient(IpAddress, Port);

                    // SCPI 통신은 Nagle 알고리즘과 Delayed Ack 사용이 오히려 지연시간을 크게 증가시키기만 하는 방해요소로 작용할 수 있음.
                    _tcp_client.NoDelay = true;

                    _connection_status = EDeviceConnectionStatus.Disconnected;

                    if (_tcp_client != null)
                    {
                        _connection_status = EDeviceConnectionStatus.Connecting;

                        _tcp_client.Connect(IpAddress, Port);

                        if (_tcp_client.Connected)
                        {
                            _connection_status = EDeviceConnectionStatus.Connected;

                            return _connection_status;
                        }
                        else
                        {
                            _connection_status = EDeviceConnectionStatus.Disconnected;

                            return _connection_status;
                        }
                    }
                }

                return _connection_status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not connect to host device. Reason: {ex.Message}");

                if (_connection_status == EDeviceConnectionStatus.Connecting)
                {
                    Func_Disconnect();
                }

                return _connection_status;
            }
        }

        public async Task<EDeviceConnectionStatus> Func_ConnectAsync(string __ipAddress, int __port, CancellationToken __cts)
        {
            try
            {
                _ip_address = __ipAddress;
                _port = __port;

                EDeviceConnectionStatus result = await Func_ConnectAsync(__cts);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not connect to host device. Reason: {ex.Message}");

                if (_connection_status == EDeviceConnectionStatus.Connecting)
                {
                    await Func_DisconnectAsync(__cts);
                }

                return _connection_status;
            }
        }

        public async Task<EDeviceConnectionStatus> Func_ConnectAsync(CancellationToken __cts)
        {
            try
            {
                if (_tcp_client is not null)
                {
                    if (_tcp_client.Connected)
                    {
                        await Func_DisconnectAsync(__cts);
                    }
                }

                _tcp_client = new TcpClient();

                // SCPI 통신은 Nagle 알고리즘과 Delayed Ack 사용이 오히려 지연시간을 크게 증가시키기만 하는 방해요소로 작용할 수 있음.
                _tcp_client.NoDelay = true;
                
                _connection_status = EDeviceConnectionStatus.Disconnected;

                if (_tcp_client != null)
                {
                    _connection_status = EDeviceConnectionStatus.Connecting;

                    await _tcp_client.ConnectAsync(IPAddress.Parse(IpAddress), Port);

                    if (_tcp_client.Connected)
                    {
                        _connection_status = EDeviceConnectionStatus.Connected;

                        return _connection_status;
                    }
                    else
                    {
                        _connection_status = EDeviceConnectionStatus.Disconnected;

                        return _connection_status;
                    }
                }

                return _connection_status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not connect to host device. Reason: {ex.Message}");

                if (_connection_status == EDeviceConnectionStatus.Connecting)
                {
                    await Func_DisconnectAsync(__cts);
                }

                return _connection_status;
            }
        }

        public EDeviceConnectionStatus Func_Disconnect()
        {
            try
            {
                if (_tcp_client == null)
                {
                    throw new Exception($"No connection to the host");
                }

                _connection_status = EDeviceConnectionStatus.Disconnecting;

                _tcp_client.Dispose();
                _tcp_client = null;

                _connection_status = EDeviceConnectionStatus.Disconnected;

                return _connection_status;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not disconnect from host device. Reason: {ex.Message}");

                return _connection_status;
            }
        }

        public async Task<EDeviceConnectionStatus> Func_DisconnectAsync(CancellationToken __cts)
        {
            try
            {
                return Func_Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can not disconnect from host device. Reason: {ex.Message}");

                return _connection_status;
            }
        }

        public string Func_Load(string __filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(__filePath))
                {
                    throw new ArgumentException("File path cannot be null or empty.");
                }

                if (!File.Exists(__filePath))
                {
                    throw new FileNotFoundException($"File not found: {__filePath}");
                }

                string json_content = File.ReadAllText(__filePath);
                return json_content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading device model from file {__filePath}. Reason: {ex.Message}");
                return string.Empty;
            }
        }

        public string Func_ReceiveCommand(uint __timeout)
        {
            try
            {
                if (_tcp_client == null || !_tcp_client.Connected)
                {
                    throw new Exception($"No connection made to host.");
                }

                if (_tcp_client.GetStream() is not null)
                {
                    NetworkStream stream = _tcp_client.GetStream();
                    if (stream.CanRead)
                    {
                        byte[] buffer = new byte[16134];
                        int readed_count = stream.Read(buffer, 0, 16134);

                        if (readed_count <= 0)
                        {
                            throw new Exception($"There is nothing to read.");
                        }

                        return Encoding.ASCII.GetString(buffer, 0, readed_count);
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

        public async Task<string> Func_ReceiveCommandAsync(CancellationToken __cts)
        {
            try
            {
                if (_tcp_client == null || !_tcp_client.Connected)
                {
                    throw new Exception($"No connection made to host.");
                }

                if (_tcp_client.GetStream() is not null)
                {
                    NetworkStream stream = _tcp_client.GetStream();
                    if (stream.CanRead)
                    {
                        byte[] buffer = new byte[16134];
                        int readed_count = await stream.ReadAsync(buffer, 0, 16134);

                        if (readed_count <= 0)
                        {
                            throw new Exception($"There is nothing to read.");
                        }

                        string response_string = Encoding.ASCII.GetString(buffer, 0, readed_count);
                        return response_string.Trim();
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

        public void Func_Save(string __filePath, string __jsonContent, bool __isBinary = false)
        {
            try
            {
                if (string.IsNullOrEmpty(__filePath))
                {
                    throw new ArgumentException("File path cannot be null or empty.");
                }

                File.WriteAllText(__filePath, __jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving device model to file {__filePath}. Reason: {ex.Message}");
            }
        }

        public void Func_SendCommand(string __command, bool __isQueryCommand)
        {
            try
            {
                if (_tcp_client == null || _tcp_client.Connected == false)
                {
                    throw new Exception($"Not connected to host.");
                }

                if (_tcp_client.GetStream() is not null)
                {
                    NetworkStream stream = _tcp_client.GetStream();
                    stream.Write(Encoding.ASCII.GetBytes(__command));
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't send a command {__command} Reason: {ex.Message}");
            }
        }

        public async Task Func_SendCommandAsync(string __command, bool __isQueryCommand, CancellationToken __cts)
        {
            try
            {
                if (_tcp_client == null || _tcp_client.Connected == false)
                {
                    throw new Exception($"Not connected to host.");
                }

                if (_tcp_client.GetStream() is not null)
                {
                    NetworkStream stream = _tcp_client.GetStream();
                    stream.WriteAsync(Encoding.ASCII.GetBytes(__command));
                    stream.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't send a command {__command} Reason: {ex.Message}");
            }
        }
    }
}