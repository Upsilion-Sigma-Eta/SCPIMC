using System.ComponentModel;

namespace SCPIMCMain.Common.Logic
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChangedEventHandler(object __sender, string? __name)
        {
            PropertyChanged?.Invoke(__sender, new PropertyChangedEventArgs(__name));
        }
    }
}