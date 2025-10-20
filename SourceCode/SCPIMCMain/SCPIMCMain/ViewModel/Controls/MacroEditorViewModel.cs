using System.Collections.ObjectModel;
using System.Windows.Input;
using SCPIMCMain.Common.Enum;
using SCPIMCMain.Common.Logic;
using SCPIMCMain.Model.Implementation;

namespace SCPIMCMain.ViewModel.Controls
{
    public class MacroEditorViewModel : NotifyPropertyChanged
    {
        private string _macro_name;
        private string _macro_description;
        private int _macro_hotkey_key_code;
        private ObservableCollection<CommandItemViewModel> _commands;
        private CommandItemViewModel _selected_command;

        public MacroEditorViewModel()
        {
            _macro_name = string.Empty;
            _macro_description = string.Empty;
            _macro_hotkey_key_code = 0;
            _commands = new ObservableCollection<CommandItemViewModel>();

            AddCommandCommand = new RelayCommand(Func_AddCommand, null);
            RemoveCommandCommand = new RelayCommand(Func_RemoveCommand, null);
        }

        public MacroEditorViewModel(MacroModel __existingMacro) : this()
        {
            if (__existingMacro != null)
            {
                _macro_name = __existingMacro.Name;
                _macro_description = __existingMacro.Description;
                _macro_hotkey_key_code = __existingMacro.HotkeyKeyCode;

                foreach (var command in __existingMacro.CommandChain)
                {
                    _commands.Add(new CommandItemViewModel(command.Item1, command.Item2));
                }
            }
        }

        public string MacroName
        {
            get
            {
                return _macro_name;
            }
            set
            {
                if (_macro_name != value)
                {
                    _macro_name = value;
                    OnPropertyChangedEventHandler(this, nameof(MacroName));
                }
            }
        }

        public string MacroDescription
        {
            get
            {
                return _macro_description;
            }
            set
            {
                if (_macro_description != value)
                {
                    _macro_description = value;
                    OnPropertyChangedEventHandler(this, nameof(MacroDescription));
                }
            }
        }

        public int MacroHotkeyKeyCode
        {
            get
            {
                return _macro_hotkey_key_code;
            }
            set
            {
                if (_macro_hotkey_key_code != value)
                {
                    _macro_hotkey_key_code = value;
                    OnPropertyChangedEventHandler(this, nameof(MacroHotkeyKeyCode));
                }
            }
        }

        public ObservableCollection<CommandItemViewModel> Commands
        {
            get
            {
                return _commands;
            }
        }

        public CommandItemViewModel SelectedCommand
        {
            get
            {
                return _selected_command;
            }
            set
            {
                if (_selected_command != value)
                {
                    _selected_command = value;
                    OnPropertyChangedEventHandler(this, nameof(SelectedCommand));
                }
            }
        }

        public ICommand AddCommandCommand { get; set; }
        public ICommand RemoveCommandCommand { get; set; }

        private void Func_AddCommand(object? __parameter)
        {
            var new_command = new CommandItemViewModel(ECommandType.Query, string.Empty);
            _commands.Add(new_command);
        }

        private void Func_RemoveCommand(object? __parameter)
        {
            try
            {
                if (_selected_command != null && _commands.Contains(_selected_command))
                {
                    _commands.Remove(_selected_command);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't remove command. Reason: {ex.Message}");
            }
        }

        public MacroModel Func_CreateMacroModel()
        {
            var macro = new MacroModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = _macro_name,
                Description = _macro_description,
                HotkeyKeyCode = _macro_hotkey_key_code,
                CommandChain = new List<(ECommandType, string)>()
            };

            foreach (var command in _commands)
            {
                macro.CommandChain.Add(command.Func_ToTuple());
            }

            return macro;
        }

        public void Func_UpdateMacroModel(MacroModel __macro)
        {
            if (__macro == null)
            {
                throw new ArgumentNullException(nameof(__macro));
            }

            __macro.Name = _macro_name;
            __macro.Description = _macro_description;
            __macro.HotkeyKeyCode = _macro_hotkey_key_code;

            __macro.CommandChain.Clear();
            foreach (var command in _commands)
            {
                __macro.CommandChain.Add(command.Func_ToTuple());
            }
        }
    }
}
