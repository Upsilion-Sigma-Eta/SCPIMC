using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Newtonsoft.Json;
using SCPIMCMain.Common.Logic;
using SCPIMCMain.Model.Implementation;
using SCPIMCMain.View.Controls;

namespace SCPIMCMain.ViewModel.Controls;

public class MacroManagementViewModel : NotifyPropertyChanged
{
    private const string MACRO_DIRECTORY = "./Macros";

    public MacroManagementViewModel()
    {
        MacroModels = new ObservableCollection<MacroModel>();

        AddMacroCommand = new RelayCommand(Func_AddMacro, null);
        DeleteMacroCommand = new RelayCommand(Func_DeleteMacro, null);
        EditMacroCommand = new RelayCommand(Func_EditMacro, null);
        ReloadMacroCommand = new RelayCommand(Func_ReloadMacro, null);
        ExecuteMacroCommand = new RelayCommand(Func_ExecuteMacro, null);

        // 저장된 매크로들을 불러옴
        Func_LoadMacros();
    }

    private ObservableCollection<MacroModel> _macro_models;
    private MacroModel _selected_macro;

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

    public MacroModel SelectedMacro
    {
        get
        {
            return _selected_macro;
        }
        set
        {
            if (_selected_macro != value)
            {
                _selected_macro = value;
                OnPropertyChangedEventHandler(this, nameof(SelectedMacro));
            }
        }
    }

    private ICommand _add_macro_command;
    private ICommand _delete_macro_commmand;
    private ICommand _edit_macro_commandl;
    private ICommand _reload_macro_command;
    private ICommand _execute_macro_command;

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

    public ICommand ExecuteMacroCommand
    {
        get
        {
            return _execute_macro_command;
        }
        set
        {
            _execute_macro_command = value;
            OnPropertyChangedEventHandler(this, nameof(ExecuteMacroCommand));
        }
    }

    public void Func_AddMacro(object? __param)
    {
        try
        {
            // 새 매크로 편집 ViewModel 생성
            var editor_view_model = new MacroEditorViewModel();

            // 다이얼로그 생성 및 표시
            var dialog = new MacroEditorDialog(editor_view_model);
            dialog.ShowDialog();

            // 사용자가 저장을 선택한 경우
            if (dialog.DialogResult_Success)
            {
                var new_macro = editor_view_model.Func_CreateMacroModel();
                MacroModels.Add(new_macro);

                // 파일로 저장
                new_macro.Func_SaveToFile(MACRO_DIRECTORY);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't add macro. Reason: {ex.Message}");
        }
    }

    public void Func_DeleteMacro(object? __param)
    {
        try
        {
            if (_selected_macro == null)
            {
                Console.WriteLine("No macro selected for deletion.");
                return;
            }

            // 파일 삭제
            string file_path = _selected_macro.Func_GetFilePath(MACRO_DIRECTORY);
            if (File.Exists(file_path))
            {
                File.Delete(file_path);
            }

            // 컬렉션에서 제거
            MacroModels.Remove(_selected_macro);
            _selected_macro = null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't Delete Selected Macro. Reason: {ex.Message}");
        }
    }

    public void Func_EditMacro(object? __param)
    {
        try
        {
            if (_selected_macro == null)
            {
                Console.WriteLine("No macro selected for editing.");
                return;
            }

            // 기존 매크로로 편집 ViewModel 생성
            var editor_view_model = new MacroEditorViewModel(_selected_macro);

            // 다이얼로그 생성 및 표시
            var dialog = new MacroEditorDialog(editor_view_model);
            dialog.ShowDialog();

            // 사용자가 저장을 선택한 경우
            if (dialog.DialogResult_Success)
            {
                // 기존 파일 삭제 (이름이 변경될 수 있으므로)
                string old_file_path = _selected_macro.Func_GetFilePath(MACRO_DIRECTORY);
                if (File.Exists(old_file_path))
                {
                    File.Delete(old_file_path);
                }

                // 매크로 업데이트
                editor_view_model.Func_UpdateMacroModel(_selected_macro);

                // 새 파일로 저장
                _selected_macro.Func_SaveToFile(MACRO_DIRECTORY);

                // UI 업데이트를 위해 프로퍼티 변경 알림
                OnPropertyChangedEventHandler(this, nameof(MacroModels));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't edit macro. Reason: {ex.Message}");
        }
    }

    public void Func_ReloadMacro(object? __param)
    {
        try
        {
            if (_selected_macro == null)
            {
                Console.WriteLine("No macro selected for reload.");
                return;
            }

            string file_path = _selected_macro.Func_GetFilePath(MACRO_DIRECTORY);

            if (File.Exists(file_path))
            {
                // 파일에서 다시 로드
                var reloaded_macro = MacroModel.Func_LoadFromFile(file_path);

                // 컬렉션에서 인덱스 찾기
                int index = MacroModels.IndexOf(_selected_macro);
                if (index >= 0)
                {
                    MacroModels[index] = reloaded_macro;
                    _selected_macro = reloaded_macro;
                }
            }
            else
            {
                Console.WriteLine($"Macro file not found: {file_path}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't Reload Macro. Reason: {ex.Message}");
        }
    }

    public void Func_ExecuteMacro(object? __param)
    {
        try
        {
            MacroModel macro_to_execute = null;

            // 파라미터가 int인 경우 (더블클릭에서 인덱스 전달)
            if (__param is int index)
            {
                if (index >= 0 && index < MacroModels.Count)
                {
                    macro_to_execute = MacroModels[index];
                }
            }
            // 파라미터가 MacroModel인 경우
            else if (__param is MacroModel macro_model)
            {
                macro_to_execute = macro_model;
            }
            // 파라미터가 없으면 선택된 매크로 사용
            else if (_selected_macro != null)
            {
                macro_to_execute = _selected_macro;
            }

            if (macro_to_execute != null)
            {
                macro_to_execute.Func_ExecuteMacro();
            }
            else
            {
                Console.WriteLine("No macro selected or parameter is invalid for execution.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can't execute macro. Reason: {ex.Message}");
        }
    }

    /// <summary>
    /// Load all macros from the Macros directory
    /// </summary>
    private void Func_LoadMacros()
    {
        try
        {
            // 디렉토리가 존재하지 않으면 생성
            if (!Directory.Exists(MACRO_DIRECTORY))
            {
                Directory.CreateDirectory(MACRO_DIRECTORY);
                return;
            }

            // 모든 JSON 파일 로드
            var json_files = Directory.GetFiles(MACRO_DIRECTORY, "*.json");

            foreach (var file_path in json_files)
            {
                try
                {
                    var macro = MacroModel.Func_LoadFromFile(file_path);
                    MacroModels.Add(macro);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to load macro from {file_path}. Reason: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load macros. Reason: {ex.Message}");
        }
    }
}
