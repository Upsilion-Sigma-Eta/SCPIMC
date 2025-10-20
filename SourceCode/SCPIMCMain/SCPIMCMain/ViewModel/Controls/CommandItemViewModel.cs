using SCPIMCMain.Common.Enum;
using SCPIMCMain.Common.Logic;

namespace SCPIMCMain.ViewModel.Controls
{
    public class CommandItemViewModel : NotifyPropertyChanged
    {
        private ECommandType _command_type;
        private string _command_text;

        public CommandItemViewModel()
        {
            _command_type = ECommandType.Query;
            _command_text = string.Empty;
        }

        public CommandItemViewModel(ECommandType __commandType, string __commandText)
        {
            _command_type = __commandType;
            _command_text = __commandText ?? string.Empty;
        }

        public ECommandType CommandType
        {
            get
            {
                return _command_type;
            }
            set
            {
                if (_command_type != value)
                {
                    _command_type = value;
                    OnPropertyChangedEventHandler(this, nameof(CommandType));
                }
            }
        }

        public string CommandText
        {
            get
            {
                return _command_text;
            }
            set
            {
                if (_command_text != value)
                {
                    _command_text = value;
                    OnPropertyChangedEventHandler(this, nameof(CommandText));
                }
            }
        }

        public (ECommandType, string) Func_ToTuple()
        {
            return (_command_type, _command_text);
        }
    }
}
