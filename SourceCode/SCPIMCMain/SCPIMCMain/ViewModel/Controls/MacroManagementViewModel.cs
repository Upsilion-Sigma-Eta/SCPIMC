using System.Collections.ObjectModel;
using System.Windows.Input;
using SCPIMCMain.Common.Logic;
using SCPIMCMain.Model.Implementation;

namespace SCPIMCMain.ViewModel.Controls;

public class MacroManagementViewModel : NotifyPropertyChanged
{
    public MacroManagementViewModel()
    {
        // 저장된 매크로들을 불러오는 로직 작성
        MacroModels = new ObservableCollection<MacroModel>();
        DeleteMacroCommand = new RelayCommand(Func_DeleteMacro, null);

    }

    private ObservableCollection<MacroModel> _macro_models;

    public ObservableCollection<MacroModel> MacroModels
    {
        get
        {
            return _macro_models;
        }
        set
        {
            _macro_models = value;
            OnPropertyChangedEventHandler(this, nameof(MacroModels));
        }
    }

    private ICommand _add_macro_command;
    private ICommand _delete_macro_commmand;
    private ICommand _edit_macro_commandl;
    private ICommand _reload_macro_command;

    public ICommand AddMacroCommand
    {
        get
        {
            return _add_macro_command;
        }
        set
        {
            _add_macro_command = value;
            OnPropertyChangedEventHandler(this, nameof(AddMacroCommand));
        }
    }

    public ICommand DeleteMacroCommand
    {
        get
        {
            return _delete_macro_commmand;
        }
        set
        {
            _delete_macro_commmand = value;
            OnPropertyChangedEventHandler(this, nameof(DeleteMacroCommand));
        }
    }

    public ICommand EditMacroCommand
    {
        get
        {
            return _edit_macro_commandl;
        }
        set
        {
            _edit_macro_commandl = value;
            OnPropertyChangedEventHandler(this, nameof(EditMacroCommand));
        }
    }

    public ICommand ReloadMacroCommand
    {
        get
        {
            return _reload_macro_command;
        }
        set
        {
            _reload_macro_command = value;
            OnPropertyChangedEventHandler(this, nameof(ReloadMacroCommand));
        }
    }

    public void Func_AddMacro(object? __param)
    {
        // Macro 설정 창 열기
    }

    public void Func_DeleteMacro(object? __param)
    {
        try
        {
            if (__param is int index)
            {
                if (index < 0 || index >= MacroModels.Count)
                {
                    throw new ArgumentException($"Index {index} is out of range for MacroModels collection.");
                }

                MacroModels.RemoveAt(index);
            }
            else
            {
                throw new ArgumentException($"Parameter must be of type int, but was {__param.GetType()}");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't Delete Selected Macro. Reason: {ex.Message}");
        }
    }

    public void Func_EditMacro(object? __param)
    {
        // Macro 설정 창 열기
    }

    public void Func_ReloadMacro(object? __param)
    {
        try
        {
            if (__param is int index)
            {
                if (index < 0 || index >= MacroModels.Count)
                {
                    throw new ArgumentException($"Index {index} is out of range for MacroModels collection.");
                }

                MacroModels[index].Func_ReloadMacro();
            }
            else
            {
                throw new ArgumentException($"Parameter must be of type int, but was {__param.GetType()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't Reload Macro. Reason: {ex.Message}");
        }
    }
}
