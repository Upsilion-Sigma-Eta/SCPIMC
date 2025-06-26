using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using SCPIMCMain.Model.Implementation;
using SCPIMCMain.Model.Interface;
using SCPIMCMain.Common.Enum;
using SCPIMCMain.Common.Logic;
using System.Collections.ObjectModel;
using SCPIMCMain.Model.Logic;
using SCPIMCMain.ViewModel.Controls;
using SCPIMCMain.View.Controls;
using System.Threading;
using System.Threading.Tasks;

namespace SCPIMCMain.ViewModel
{
    public class MainControlPanelViewModel : NotifyPropertyChanged
    {
        public MainControlPanelViewModel()
        {
            _deviceModel = new DeviceModel();
            SendedMessageLogPanel = new LogPanelViewModel(ELogPanelKeys.MainSendMessageLog);
            ReceivedMessageLogPanel = new LogPanelViewModel(ELogPanelKeys.MainReceivedMessageLog);

            Singleton<ManagerService<int, DeviceModel>>.Instance.AddKeyWithValue(new KeyValuePair<int, DeviceModel>(0, _deviceModel));

            DeviceIPAddress = "0.0.0.0";
            DevicePortNum = "0";
            _tabItemModelCollection = new ObservableCollection<TabItemModel>();

            ManagerService<ELogPanelKeys, LogPanelViewModel> managerSerivce = Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance;

            managerSerivce.AddKeyWithValue(new KeyValuePair<ELogPanelKeys, LogPanelViewModel>(ELogPanelKeys.MainSendMessageLog, SendedMessageLogPanel));
            managerSerivce.AddKeyWithValue(new KeyValuePair<ELogPanelKeys, LogPanelViewModel>(ELogPanelKeys.MainReceivedMessageLog, ReceivedMessageLogPanel));

            managerSerivce.AddKeyWithValue(new KeyValuePair<ELogPanelKeys, LogPanelViewModel>(ELogPanelKeys.ProgramLog, new LogPanelViewModel(ELogPanelKeys.ProgramLog)));
            managerSerivce.AddKeyWithValue(new KeyValuePair<ELogPanelKeys, LogPanelViewModel>(ELogPanelKeys.CommunicationLog, new LogPanelViewModel(ELogPanelKeys.CommunicationLog)));

            TabItemModel logTab = new TabItemModel("Program Log", true, managerSerivce.TryGetValue(ELogPanelKeys.ProgramLog));
            TabItemModel commLogTab = new TabItemModel("Comm Log", true, managerSerivce.TryGetValue(ELogPanelKeys.CommunicationLog));

            SelectedTabItemModel = logTab;

            TabItemModelCollection.Add(logTab);
            TabItemModelCollection.Add(commLogTab);

            managerSerivce.TryGetValue(ELogPanelKeys.ProgramLog).Log("Program Intialized.");

            DeviceConnectCommand = new RelayCommand(new Action<object?>(async (object? parameter) => await ConnectDevice(parameter)), null);
        }

        private LogPanelViewModel _sendedMessageLogPanel;
        public LogPanelViewModel SendedMessageLogPanel
        {
            get
            {
                return _sendedMessageLogPanel;
            }
            set
            {
                _sendedMessageLogPanel = value;
                OnPropertyChangedEventHandler(this, nameof(SendedMessageLogPanel));
            }
        }

        private LogPanelViewModel _receivedMessageLogPanel;
        private LogPanelViewModel ReceivedMessageLogPanel
        {
            get
            {
                return _receivedMessageLogPanel;
            }
            set
            {
                _receivedMessageLogPanel = value;
                OnPropertyChangedEventHandler(this, nameof(ReceivedMessageLogPanel));
            }
        }

        // ===== Variable and Properties =====
        private TabItemModel _selectedTabItemModel;
        public TabItemModel SelectedTabItemModel
        {
            get
            {
                return _selectedTabItemModel;
            }
            set
            {
                _selectedTabItemModel = value;
                OnPropertyChangedEventHandler(this, nameof(SelectedTabItemModel));
            }
        }

        private ObservableCollection<TabItemModel> _tabItemModelCollection;
        public ObservableCollection<TabItemModel> TabItemModelCollection
        {
            get
            {
                return _tabItemModelCollection;
            }
        }
        private DeviceModel _deviceModel;

        private ICommand _deviceConnectCommand;

        public string DeviceIPAddress
        {
            get
            {
                return _deviceModel.IPAddress;
            }
            set
            {
                if (_deviceModel.IPAddress != value)
                {
                    _deviceModel.IPAddress = value;
                    OnPropertyChangedEventHandler(this, nameof(DeviceIPAddress));
                }
            }
        }

        public string DevicePortNum
        {
            get
            {
                return _deviceModel.Port.ToString();
            }
            set
            {
                int portNumEntered;
                if (!int.TryParse(value, out portNumEntered))
                {
                    MessageBox.Show("Error", "Port number must be a integer.");

                    return;
                }

                if (_deviceModel.Port != portNumEntered)
                {
                    _deviceModel.Port = portNumEntered;
                    OnPropertyChangedEventHandler(this, nameof(DevicePortNum));
                }
            }
        }

        public ICommand DeviceConnectCommand
        {
            get
            {
                return _deviceConnectCommand;
            }
            set
            {
                _deviceConnectCommand = value;
                OnPropertyChangedEventHandler(this, nameof(DeviceConnectCommand));
            }
        }

        // ===== Method =====
        private async Task ConnectDevice(object? parameter)
        {
            try
            {
                using (CancellationTokenSource cts = new CancellationTokenSource())
                {
                    // Attempt to connect to the device asynchronously
                    await _deviceModel.ConnectAsync(DeviceIPAddress, int.Parse(DevicePortNum), cts.Token);
                }
            }
            catch (Exception ex)
            {
                Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.TryGetValue(ELogPanelKeys.ProgramLog).Log($"Error connecting to device: {ex.Message}");
            }
        }

    }
}