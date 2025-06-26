using System;
using System.Collections.ObjectModel;
using SCPIMCMain.Common.Enum;
using SCPIMCMain.Common.Logic;

namespace SCPIMCMain.ViewModel.Controls
{
    public class LogPanelViewModel : NotifyPropertyChanged
    {
        private ObservableCollection<string> _logs;
        private ELogPanelKeys _logPanelKey;

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
            _logPanelKey = ELogPanelKeys.ProgramLog;
        }

        public LogPanelViewModel(ELogPanelKeys key)
        {
            _logs = new ObservableCollection<string>();
            _logPanelKey = key;
        }

        public void Log(string log)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
            string actualLog = $"[{timestamp}] | {log}";

            Logs.Add(actualLog);

            if (Logs.Count > 300)
            {
                Logs.RemoveAt(0);
            }
        }
    }
}