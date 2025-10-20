using System.IO;
using Newtonsoft.Json;
using SCPIMCMain.Common.Enum;
using SCPIMCMain.Common.Logic;
using SCPIMCMain.Model.Interface;
using SCPIMCMain.Model.Logic;
using SCPIMCMain.ViewModel.Controls;

namespace SCPIMCMain.Model.Implementation;

public class MacroModel : IMacro, ISaveable, ILoadable
{
    private string _id;
    private string _name;
    private string _description;
    private int _hotkey_key_code;
    private List<(ECommandType, string)> _command_chain;

    public MacroModel()
    {
        _id = string.Empty;
        _name = string.Empty;
        _description = string.Empty;
        _hotkey_key_code = 0;
        _command_chain = new List<(ECommandType, string)>();
    }

    public string Id { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public string Description { get => _description; set => _description = value; }
    public int HotkeyKeyCode { get => _hotkey_key_code; set => _hotkey_key_code = value; }
    public List<(ECommandType, string)> CommandChain { get => _command_chain; set => _command_chain = value; }

    public void Func_AddCommand(ECommandType __commandType, string __command)
    {
        _command_chain.Add((__commandType, __command));
    }

    public void Func_ClearCommands()
    {
        _command_chain.Clear();
    }

    public void Func_DeleteMacro()
    {
        string path = Path.Combine(".", "/Macro", $"/{_id}|{_name}");
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        Func_ResetMacro();
        _command_chain = null;
    }

    public async void Func_ExecuteMacro()
    {
        try
        {
            // Device는 key 0으로 등록되어 있음
            DeviceModel current_connected_device = Singleton<ManagerService<int, DeviceModel>>.Instance.Func_TryGetValue(0);

            LogPanelViewModel comm_log = Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.CommunicationLog);
            LogPanelViewModel main_receive_message_log = Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.MainReceivedMessageLog);
            LogPanelViewModel program_log = Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.ProgramLog);

            program_log.Func_Log($"Macro '{Name}' Started.");

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                foreach ((ECommandType, string) command in _command_chain)
                {
                    switch (command.Item1)
                    {
                        case ECommandType.Delay:
                            int delay_ms = int.Parse(command.Item2);
                            program_log.Func_Log($"Delaying for {delay_ms}ms...");
                            await Task.Delay(delay_ms);
                            break;

                        case ECommandType.Query:
                            program_log.Func_Log($"Sending Query: {command.Item2}");
                            await current_connected_device.Func_SendCommandAsync(command.Item2, true, cts.Token);
                            string response = current_connected_device.Func_ReceiveCommand(1000);

                            comm_log.Func_Log($"Query Response: {response}");
                            main_receive_message_log.Func_Log(response);
                            break;

                        case ECommandType.Setter:
                            program_log.Func_Log($"Sending Setter: {command.Item2}");
                            await current_connected_device.Func_SendCommandAsync(command.Item2, false, cts.Token);
                            break;

                        default:
                            throw new NotImplementedException($"Command type {command.Item1} is not implemented.");
                    }
                }
            }

            program_log.Func_Log($"Macro '{Name}' Completed Successfully.");
        }
        catch (Exception ex)
        {
            var program_log = Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.Func_TryGetValue(ELogPanelKeys.ProgramLog);
            program_log.Func_Log($"Macro '{Name}' Failed: {ex.Message}");
            Console.WriteLine($"Macro execution failed. Reason: {ex.Message}");
        }
    }

    public string Func_Load(string __filePath)
    {
        if (string.IsNullOrEmpty(__filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(__filePath));
        }

        if (!File.Exists(__filePath))
        {
            throw new FileNotFoundException($"File not found: {__filePath}");
        }

        string json_content = File.ReadAllText(__filePath);
        // Deserialize the JSON content to populate the macro properties
        // Assuming a JSON deserialization method is available
        // Example: JsonConvert.DeserializeObject<MacroModel>(jsonContent);

        return json_content; // Return the loaded content or the deserialized object
    }

    public void Func_RemoveCommand(int __index)
    {
        _command_chain.RemoveAt(__index);
    }

    public void Func_ResetMacro()
    {
        _id = null;
        _command_chain.Clear();
        _name = null;
        _description = null;
        _hotkey_key_code = 0;
    }

    public void Func_Save(string __filePath, string __jsonContent, bool __isBinary = false)
    {
        if (string.IsNullOrEmpty(__filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(__filePath));
        }

        string directory = Path.GetDirectoryName(__filePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(__filePath, __jsonContent);
    }

    // 매크로를 다시 불러옴
    public void Func_ReloadMacro()
    {
        // 다시 불러오기 로직
        string path = Path.Combine(".", "/Macro", $"/{_id}|{_name}");
        if (File.Exists(path))
        {
            Func_Load(path);
        }
    }

    public void Func_ValidateMacro()
    {
        if (string.IsNullOrEmpty(_id))
        {
            throw new InvalidOperationException("Macro ID cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(_name))
        {
            throw new InvalidOperationException("Macro name cannot be null or empty.");
        }
        if (_command_chain == null || _command_chain.Count == 0)
        {
            throw new InvalidOperationException("Macro must contain at least one command.");
        }
    }

    // ===== JSON Serialization Methods =====

    /// <summary>
    /// Convert MacroModel to DTO for serialization
    /// </summary>
    public MacroDto Func_ToDto()
    {
        var dto = new MacroDto
        {
            Id = _id,
            Name = _name,
            Description = _description,
            HotkeyKeyCode = _hotkey_key_code,
            CommandChain = new List<CommandDto>()
        };

        foreach (var command in _command_chain)
        {
            dto.CommandChain.Add(new CommandDto(command.Item1, command.Item2));
        }

        return dto;
    }

    /// <summary>
    /// Load MacroModel from DTO
    /// </summary>
    public void Func_FromDto(MacroDto __dto)
    {
        if (__dto == null)
        {
            throw new ArgumentNullException(nameof(__dto));
        }

        _id = __dto.Id;
        _name = __dto.Name;
        _description = __dto.Description;
        _hotkey_key_code = __dto.HotkeyKeyCode;
        _command_chain = new List<(ECommandType, string)>();

        foreach (var command in __dto.CommandChain)
        {
            _command_chain.Add((command.CommandType, command.CommandText));
        }
    }

    /// <summary>
    /// Save macro to JSON file
    /// </summary>
    public void Func_SaveToFile(string __directoryPath)
    {
        try
        {
            if (string.IsNullOrEmpty(__directoryPath))
            {
                throw new ArgumentException("Directory path cannot be null or empty.", nameof(__directoryPath));
            }

            // Create directory if it doesn't exist
            if (!Directory.Exists(__directoryPath))
            {
                Directory.CreateDirectory(__directoryPath);
            }

            // Create file path: {directory}/{id}_{name}.json
            string safe_name = string.Join("_", _name.Split(Path.GetInvalidFileNameChars()));
            string file_path = Path.Combine(__directoryPath, $"{_id}_{safe_name}.json");

            // Convert to DTO and serialize
            var dto = Func_ToDto();
            string json_content = JsonConvert.SerializeObject(dto, Formatting.Indented);

            // Save to file
            File.WriteAllText(file_path, json_content);
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to save macro to file. Reason: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Load macro from JSON file
    /// </summary>
    public static MacroModel Func_LoadFromFile(string __filePath)
    {
        try
        {
            if (string.IsNullOrEmpty(__filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(__filePath));
            }

            if (!File.Exists(__filePath))
            {
                throw new FileNotFoundException($"Macro file not found: {__filePath}");
            }

            // Read and deserialize
            string json_content = File.ReadAllText(__filePath);
            var dto = JsonConvert.DeserializeObject<MacroDto>(json_content);

            if (dto == null)
            {
                throw new InvalidOperationException("Failed to deserialize macro from JSON.");
            }

            // Create MacroModel from DTO
            var macro = new MacroModel();
            macro.Func_FromDto(dto);

            return macro;
        }
        catch (Exception ex)
        {
            throw new IOException($"Failed to load macro from file. Reason: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Get file path for this macro
    /// </summary>
    public string Func_GetFilePath(string __directoryPath)
    {
        string safe_name = string.Join("_", _name.Split(Path.GetInvalidFileNameChars()));
        return Path.Combine(__directoryPath, $"{_id}_{safe_name}.json");
    }
}
