using System.Collections.ObjectModel;
using SCPIMCMain.Common.Logic;

namespace SCPIMCMain.ViewModel.Controls
{
    public class LogPanelViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<string> _logs;

        public ObservableCollection<string> Logs
        {
            get
            {
                return _logs;
            }
        }

        public LogPanelViewModel()
        {
            _logs = new ObservableCollection<string>();
        }
        
        public void Func_Log(string __log)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
            string actual_log = $"[{timestamp}] | {__log}";

            Logs.Add(actual_log);

            if (Logs.Count > 300)
            {
                Logs.RemoveAt(0);
            }
        }
    }
}