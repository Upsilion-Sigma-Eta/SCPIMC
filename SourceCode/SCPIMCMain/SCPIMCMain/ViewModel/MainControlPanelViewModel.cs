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
            DeviceIPAddress = "0.0.0.0";
            DevicePortNum = "0";
            _tabItemModelCollection = new ObservableCollection<TabItemModel>();

            ManagerService<ELogPanelKeys, LogPanelViewModel> managerSerivce = Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance;

            managerSerivce.AddKeyWithValue(new KeyValuePair<ELogPanelKeys, LogPanelViewModel>(ELogPanelKeys.ProgramLog, new LogPanelViewModel(ELogPanelKeys.ProgramLog)));

            TabItemModel logTab = new TabItemModel("Program Log", true, managerSerivce.TryGetValue(ELogPanelKeys.ProgramLog));

            SelectedTabItemModel = logTab;

            TabItemModelCollection.Add(logTab);

            managerSerivce.TryGetValue(ELogPanelKeys.ProgramLog).Log("Program Intialized.");

            DeviceConnectCommand = new RelayCommand(new Action<object?>(async (object? parameter) => await ConnectDevice(parameter)), null);
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