using System.ComponentModel;
using SCPIMCMain.Common.Logic;

namespace SCPIMCMain.ViewModel
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private object _content;
        public object ContentView
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChangedEventHandler(this, nameof(ContentView));
            }
        }

        public MainWindowViewModel()
        {
            ContentView = new MainControlPanelViewModel();
        }
    }
}