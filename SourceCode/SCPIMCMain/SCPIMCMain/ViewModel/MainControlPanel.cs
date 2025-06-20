using System.ComponentModel;
using System.Windows.Input;

namespace SCPIMCMain.ViewModel
{
    public class MainControlPanel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChangedEventHandler(object sender, string? name)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(name));
        }

        private string _deviceIPAddress;
        private string _devicePortNum;
        private ICommand _deviceConnectCommand;

        public string DeviceIPAddress
        {
            
        }

        public string _devicePortNum
        {

        }

        public ICommand DeviceConnectCommand
        {

        }
    }
}