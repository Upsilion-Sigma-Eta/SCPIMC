using System.Windows;
using System.Windows.Input;
using SCPIMCMain.Model.Implementation;
using SCPIMCMain.Common.Enum;
using SCPIMCMain.Common.Logic;
using System.Collections.ObjectModel;
using SCPIMCMain.Model.Logic;
using SCPIMCMain.Model.Service;
using SCPIMCMain.ViewModel.Controls;

namespace SCPIMCMain.ViewModel
{
    public class MainControlPanelViewModel : NotifyPropertyChanged
    {
        public MainControlPanelViewModel()
        {
            _device_model = new DeviceModel();
            SendedMessageLogPanel = new LogPanelViewModel();
            ReceivedMessageLogPanel = new LogPanelViewModel();

            Singleton<ManagerService<int, DeviceModel>>.Instance.Func_AddKeyWithValue(new KeyValuePair<int, DeviceModel>(0, _device_model));

            // 저장된 설정 로드
            ApplicationSettings settings = SettingsService.Func_LoadSettings();
            DeviceIpAddress = settings.LastDeviceIp;
            DevicePortNum = settings.LastDevicePort.ToString();
            _tab_item_model_collection = new ObservableCollection<TabItemModel>();

            ManagerService<ELogPanelKeys, LogPanelViewModel> manager_serivce = Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance;

            manager_serivce.Func_AddKeyWithValue(new KeyValuePair<ELogPanelKeys, LogPanelViewModel>(ELogPanelKeys.MainSendMessageLog, SendedMessageLogPanel));
            manager_serivce.Func_AddKeyWithValue(new KeyValuePair<ELogPanelKeys, LogPanelViewModel>(ELogPanelKeys.MainReceivedMessageLog, ReceivedMessageLogPanel));

            manager_serivce.Func_AddKeyWithValue(new KeyValuePair<ELogPanelKeys, LogPanelViewModel>(ELogPanelKeys.ProgramLog, new LogPanelViewModel()));
            manager_serivce.Func_AddKeyWithValue(new KeyValuePair<ELogPanelKeys, LogPanelViewModel>(ELogPanelKeys.CommunicationLog, new LogPanelViewModel()));

            TabItemModel log_tab = new TabItemModel("Program Log", true, manager_serivce.Func_TryGetValue(ELogPanelKeys.ProgramLog));
            TabItemModel comm_log_tab = new TabItemModel("Comm Log", true, manager_serivce.Func_TryGetValue(ELogPanelKeys.CommunicationLog));
            TabItemModel macro_tab = new TabItemModel("Macro", true,
                new MacroManagementViewModel());

            SelectedTabItemModel = log_tab;

            TabItemModelCollection.Add(log_tab);
            TabItemModelCollection.Add(comm_log_tab);
            TabItemModelCollection.Add(macro_tab);

            manager_serivce.Func_TryGetValue(ELogPanelKeys.ProgramLog).Func_Log("Program Intialized.");

            DeviceConnectCommand = new RelayCommand(async void (__parameter) => await Func_ConnectDevice(__parameter), null);
            DeviceSendScpiCommand = new RelayCommand(async void (__paramater) => await Func_SendSCPICommand(__paramater), null);
        }

        private LogPanelViewModel _sended_message_log_panel;
        public LogPanelViewModel SendedMessageLogPanel
        {
            get
            {
                return _sended_message_log_panel;
            }
            set
            {
                _sended_message_log_panel = value;
                OnPropertyChangedEventHandler(this, nameof(SendedMessageLogPanel));
            }
        }

        private LogPanelViewModel _received_message_log_panel;
        public LogPanelViewModel ReceivedMessageLogPanel
        {
            get
            {
                return _received_message_log_panel;
            }
            set
            {
                _received_message_log_panel = value;
                OnPropertyChangedEventHandler(this, nameof(ReceivedMessageLogPanel));
            }
        }

        // ===== Variable and Properties =====
        private TabItemModel _selected_tab_item_model;
        public TabItemModel SelectedTabItemModel
        {
            get
            {
                return _selected_tab_item_model;
            }
            set
            {
                _selected_tab_item_model = value;
                OnPropertyChangedEventHandler(this, nameof(SelectedTabItemModel));
            }
        }

        private ObservableCollection<TabItemModel> _tab_item_model_collection;
        public ObservableCollection<TabItemModel> TabItemModelCollection
        {
            get
            {
                return _tab_item_model_collection;
            }
        }
        private DeviceModel _device_model;

        private ICommand _device_connect_command;

        public string DeviceIpAddress
        {
            get
            {
                return _device_model.IpAddress;
            }
            set
            {
                if (_device_model.IpAddress != value)
                {
                    _device_model.IpAddress = value;
                    OnPropertyChangedEventHandler(this, nameof(DeviceIpAddress));
                }
            }
        }

        public string DevicePortNum
        {
            get
            {
                return _device_model.Port.ToString();
            }
            set
            {
                int port_num_entered;
                if (!int.TryParse(value, out port_num_entered))
                {
                    MessageBox.Show("Error", "Port number must be a integer.");

                    return;
                }

                if (_device_model.Port != port_num_entered)
                {
                    _device_model.Port = port_num_entered;
                    OnPropertyChangedEventHandler(this, nameof(DevicePortNum));
                }
            }
        }

        public ICommand DeviceConnectCommand
        {
            get
            {
                return _device_connect_command;
            }
            set
            {
                _device_connect_command = value;
                OnPropertyChangedEventHandler(this, nameof(DeviceConnectCommand));
            }
        }

        private ICommand _device_send_scpi_command;
        public ICommand DeviceSendScpiCommand
        {
            get
            {
                return _device_send_scpi_command;
            }
            set
            {
                _device_send_scpi_command = value;
                OnPropertyChangedEventHandler(this, nameof(DeviceSendScpiCommand));
            }
        }

        private string _entered_scpi_command;
        public string EnteredScpiCommand
        {
            get
            {
                return _entered_scpi_command;
            }
            set
            {
                if (_entered_scpi_command != value)
                {
                    _entered_scpi_command = value;
                    OnPropertyChangedEventHandler(this, nameof(EnteredScpiCommand));
                }
            }
        }

        private string _last_received_message;
        public string LastReceivedMessage
        {
            get
            {
                return _last_received_message;
            }
            set
            {
                _last_received_message = value;
                OnPropertyChangedEventHandler(this, nameof(LastReceivedMessage));
            }
        }

        // ===== Method =====
        private async Task Func_ConnectDevice(object? __parameter)
        {
            try
            {
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    // Attempt to connect to the device asynchronously
                    EDeviceConnectionStatus status = await _device_model.Func_ConnectAsync(DeviceIpAddress, int.Parse(DevicePortNum), cts.Token);

                    if (status == EDeviceConnectionStatus.Connected)
                    {
                        Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.CommunicationLog).Func_Log($"Connected to device.");

                        // 연결 성공 시 현재 IP와 Port를 설정 파일에 저장
                        ApplicationSettings settings = new ApplicationSettings(DeviceIpAddress, int.Parse(DevicePortNum));
                        SettingsService.Func_SaveSettings(settings);
                    }
                    else
                    {
                        Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.CommunicationLog).Func_Log($"Can not connect to device.");
                    }
                }
            }
            catch (Exception ex)
            {
                Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.ProgramLog).Func_Log($"Error connecting to device: {ex.Message}");
            }
        }

        private async Task Func_SendSCPICommand(object? __parameter)
        {
            try
            {
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    bool is_query_command = EnteredScpiCommand.Contains('?');
                    
                    await _device_model.Func_SendCommandAsync($"{EnteredScpiCommand}\r\n", is_query_command, cts.Token);
                    
                    Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.CommunicationLog).Func_Log($"Sent SCPI command: {EnteredScpiCommand}");
                    Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.MainSendMessageLog).Func_Log(EnteredScpiCommand);

                    if (is_query_command)
                    {
                        LastReceivedMessage = await _device_model.Func_ReceiveCommandAsync(cts.Token);
                        Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.CommunicationLog)
                            .Func_Log($"Recieved SCPi Response : {LastReceivedMessage}");
                        Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.MainReceivedMessageLog).Func_Log(LastReceivedMessage);
                    }
                }
            }
            catch (Exception e)
            {
                Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.ProgramLog).Func_Log($"Error sending SCPI command: {e.Message}");
            }
        }

    }
}