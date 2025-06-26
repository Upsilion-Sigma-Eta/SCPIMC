using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using SCPIMCMain.Model.Implementation;
using SCPIMCMain.Model.Interface;
using SCPIMCMain.Common.Enum;
using SCPIMCMain.Common.Logic;
using System.Collections.ObjectModel;
using SCPIMCMain.Model.Logic;

namespace SCPIMCMain.ViewModel
{
    public class MainControlPanelViewModel : NotifyPropertyChanged
    {
        public MainControlPanelViewModel()
        {
            _deviceModel = new DeviceModel();
            DeviceIPAddress = "0.0.0.0";
            DevicePortNum = "0";

            TabItemModel logTab = new TabItemModel("Program Log", true);

            DeviceConnectCommand = new RelayCommand(new Action<object?>((object? parameter) => ConnectDevice(parameter)), null);
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
                Console.WriteLine($"Error connecting to device: {ex.Message}");
            }
        }

    }
}