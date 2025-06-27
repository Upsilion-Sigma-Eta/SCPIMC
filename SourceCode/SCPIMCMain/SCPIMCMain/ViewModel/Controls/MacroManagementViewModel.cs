using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
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
        DeleteMacroCommand = new RelayCommand(DeleteMacro, null);

    }

    private ObservableCollection<MacroModel> _macroModels;

    public ObservableCollection<MacroModel> MacroModels
    {
        get
        {
            return _macroModels;
        }
        set
        {
            _macroModels = value;
            OnPropertyChangedEventHandler(this, nameof(MacroModels));
        }
    }

    private ICommand _addMacroCommand;
    private ICommand _deleteMacroCommmand;
    private ICommand _editMacroCommandl;
    private ICommand _reloadMacroCommand;

    public ICommand AddMacroCommand
    {
        get
        {
            return _addMacroCommand;
        }
        set
        {
            _addMacroCommand = value;
            OnPropertyChangedEventHandler(this, nameof(AddMacroCommand));
        }
    }

    public ICommand DeleteMacroCommand
    {
        get
        {
            return _deleteMacroCommmand;
        }
        set
        {
            _deleteMacroCommmand = value;
            OnPropertyChangedEventHandler(this, nameof(DeleteMacroCommand));
        }
    }

    public ICommand EditMacroCommand
    {
        get
        {
            return _editMacroCommandl;
        }
        set
        {
            _editMacroCommandl = value;
            OnPropertyChangedEventHandler(this, nameof(EditMacroCommand));
        }
    }

    public ICommand ReloadMacroCommand
    {
        get
        {
            return _reloadMacroCommand;
        }
        set
        {
            _reloadMacroCommand = value;
            OnPropertyChangedEventHandler(this, nameof(ReloadMacroCommand));
        }
    }

    public void AddMacro(object? param)
    {
        // Macro 설정 창 열기
    }

    public void DeleteMacro(object? param)
    {
        try
        {
            if (param is int index)
            {
                if (index < 0 || index >= MacroModels.Count)
                {
                    throw new ArgumentException($"Index {index} is out of range for MacroModels collection.");
                }

                MacroModels.RemoveAt(index);
            }
            else
            {
                throw new ArgumentException($"Parameter must be of type int, but was {param.GetType()}");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't Delete Selected Macro. Reason: {ex.Message}");
        }
    }

    public void EditMacro(object? param)
    {
        // Macro 설정 창 열기
    }

    public void ReloadMacro(object? param)
    {
        try
        {
            if (param is int index)
            {
                if (index < 0 || index >= MacroModels.Count)
                {
                    throw new ArgumentException($"Index {index} is out of range for MacroModels collection.");
                }

                MacroModels[index].ReloadMacro();
            }
            else
            {
                throw new ArgumentException($"Parameter must be of type int, but was {param.GetType()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't Reload Macro. Reason: {ex.Message}");
        }
    }
}
