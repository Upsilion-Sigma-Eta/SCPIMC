using System.Collections.Specialized;
using System.ComponentModel;

namespace SCPIMCMain.Common.Logic
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChangedEventHandler(object sender, string? name)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(name));
        }
    }
}