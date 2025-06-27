using System;
using System.Collections.Generic;
using System.IO;
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
    private int _hotkeyKeyCode;
    private List<(ECommandType, string)> _commandChain;

    public string Id { get => _id; set => _id = value; }
    public string Name { get => _name; set => _name = value; }
    public string Description { get => _description; set => _description = value; }
    public int HotkeyKeyCode { get => _hotkeyKeyCode; set => _hotkeyKeyCode = value; }
    public List<(ECommandType, string)> CommandChain { get => _commandChain; set => _commandChain = value; }

    public void AddCommand(ECommandType commandType, string command)
    {
        _commandChain.Add((commandType, command));
    }

    public void ClearCommands()
    {
        _commandChain.Clear();
    }

    public void DeleteMacro()
    {
        string path = Path.Combine(".", "/Macro", $"/{_id}|{_name}");
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        ResetMacro();
        _commandChain = null;
    }

    public async void ExecuteMacro()
    {
        DeviceModel _currentConnectedDevice = Singleton<ManagerService<int, DeviceModel>>.Instance.TryGetValue(int.Parse(_id));

        LogPanelViewModel commLog = Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.TryGetValue(ELogPanelKeys.CommunicationLog);
        LogPanelViewModel mainReceiveMessageLog = Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.TryGetValue(ELogPanelKeys.MainReceivedMessageLog);
        LogPanelViewModel programLog = Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance.TryGetValue(ELogPanelKeys.ProgramLog);

        programLog.Log($"Macro {Name} Started.");

        using (CancellationTokenSource cts = new CancellationTokenSource())
        {
            foreach ((ECommandType, string) command in _commandChain)
            {
                switch (command.Item1)
                {
                    case ECommandType.Delay:
                        await Task.Delay(int.Parse(command.Item2));
                        break;
                    case ECommandType.Query:
                        await _currentConnectedDevice.SendCommandAsync(command.Item2, true, cts.Token);
                        string response = _currentConnectedDevice.ReceiveCommand(1000);

                        commLog.Log(response);
                        mainReceiveMessageLog.Log(response);
                        break;
                    case ECommandType.Setter:
                        await _currentConnectedDevice.SendCommandAsync(command.Item2, false, cts.Token);
                        break;
                    default:
                        throw new NotImplementedException($"Command type {command.Item1} is not implemented.");
                }
            }
        }

        programLog.Log($"Macro {Name} End.");
    }

    public string Load(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        string jsonContent = File.ReadAllText(filePath);
        // Deserialize the JSON content to populate the macro properties
        // Assuming a JSON deserialization method is available
        // Example: JsonConvert.DeserializeObject<MacroModel>(jsonContent);

        return jsonContent; // Return the loaded content or the deserialized object
    }

    public void RemoveCommand(int index)
    {
        _commandChain.RemoveAt(index);
    }

    public void ResetMacro()
    {
        _id = null;
        _commandChain.Clear();
        _name = null;
        _description = null;
        _hotkeyKeyCode = 0;
    }

    public void Save(string filePath, string jsonContent, bool isBinary = false)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
        }

        string directory = Path.GetDirectoryName(filePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(filePath, jsonContent);
    }

    // 매크로를 다시 불러옴
    public void ReloadMacro()
    {
        // 다시 불러오기 로직
        string path = Path.Combine(".", "/Macro", $"/{_id}|{_name}");
        if (File.Exists(path))
        {
            Load(path);
        }
    }

    public void ValidateMacro()
    {
        if (string.IsNullOrEmpty(_id))
        {
            throw new InvalidOperationException("Macro ID cannot be null or empty.");
        }
        if (string.IsNullOrEmpty(_name))
        {
            throw new InvalidOperationException("Macro name cannot be null or empty.");
        }
        if (_commandChain == null || _commandChain.Count == 0)
        {
            throw new InvalidOperationException("Macro must contain at least one command.");
        }
    }
}
