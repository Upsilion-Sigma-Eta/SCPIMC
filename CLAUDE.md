# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

SCPIMC is a WPF-based desktop application for communicating with measurement equipment using the SCPI (Standard Commands for Programmable Instruments) protocol. The application enables users to send SCPI commands, monitor responses, and manage automated command sequences (macros) through TCP/IP connections.

## Build and Run Commands

### Build
```bash
cd SourceCode/SCPIMCMain
dotnet build SCPIMCMain.sln
```

For Release build:
```bash
dotnet build SCPIMCMain.sln -c Release
```

### Run
```bash
cd SourceCode/SCPIMCMain/SCPIMCMain
dotnet run
```

Or run the built executable:
```bash
cd SourceCode/SCPIMCMain/SCPIMCMain/bin/Debug/net8.0-windows
./SCPIMCMain.exe
```

### Clean
```bash
cd SourceCode/SCPIMCMain
dotnet clean SCPIMCMain.sln
```

## Technology Stack

- **.NET 8.0 Windows** (WPF application)
- **Target Framework**: net8.0-windows
- **Dependencies**:
  - Newtonsoft.Json (v13.0.3) - JSON serialization for settings and macros
- **Architecture**: MVVM (Model-View-ViewModel)

## Architecture Overview

### MVVM Pattern

The application follows a strict MVVM architecture:

- **Model** (`Model/` directory): Business logic, device communication, and data persistence
  - `DeviceModel`: Manages TCP/IP connections and SCPI command transmission/reception
  - `MacroModel`: Handles automated command sequences with delays and command chains
  - Services use generic `ManagerService<TKey, TValue>` for centralized resource management

- **View** (`View/` directory): XAML-based UI components
  - Main window uses a 3-column layout: control panel, sent messages log, received messages log
  - Tab-based interface for extensibility (Program Log, Comm Log, Macro management)

- **ViewModel** (`ViewModel/` directory): UI logic and data binding
  - `MainControlPanelViewModel`: Coordinates device connections and log panels
  - Uses `RelayCommand` for ICommand implementations
  - `LogPanelViewModel`: Manages log display for different log types

### Key Design Patterns

1. **Singleton Pattern**: `Singleton<T>` class used for global service access
   - Example: `Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance`
   - Used for centralized access to DeviceModel and LogPanelViewModel instances

2. **Service Pattern**: `ManagerService<TKey, TValue>` provides generic dictionary-based resource management
   - Manages DeviceModel instances by integer keys
   - Manages LogPanelViewModel instances by ELogPanelKeys enum
   - Provides `Func_TryGetValue()`, `Func_AddKeyWithValue()`, `Func_RemoveKeyWithValue()` methods

3. **Command Pattern**: Macro system uses command chains with `(ECommandType, string)` tuples
   - `ECommandType.Setter`: Send SCPI command without expecting response
   - `ECommandType.Query`: Send SCPI query and wait for response
   - `ECommandType.Delay`: Pause execution for specified milliseconds

4. **Interface Segregation**: Models implement focused interfaces
   - `ISaveable`: Provides `Func_Save()` for JSON serialization
   - `ILoadable`: Provides `Func_Load()` for JSON deserialization
   - `IDeviceModel`: Defines device connection and communication contract
   - `IMacro`: Defines macro execution and management contract

### Communication Architecture

Device communication is handled through `DeviceModel`:

- **Synchronous Methods**: `Func_Connect()`, `Func_SendCommand()`, `Func_ReceiveCommand()`
- **Asynchronous Methods**: `Func_ConnectAsync()`, `Func_SendCommandAsync()`, `Func_ReceiveCommandAsync()`
- Uses `TcpClient` for TCP/IP connections
- ASCII encoding for SCPI command transmission
- Connection state tracking via `EDeviceConnectionStatus` enum

### File Persistence

- `FileSaverAndLoaderService`: Handles file I/O operations
- Macros saved to `./Macro/` directory with naming pattern `{id}|{name}`
- JSON format for settings and macro serialization using Newtonsoft.Json

## Development Guidelines

### Naming Conventions

- **Hungarian Notation for Parameters**: Parameters prefixed with `__` (e.g., `__ipAddress`, `__command`, `__filePath`)
- **Private Fields**: Underscore prefix (e.g., `_tcp_client`, `_device_model`, `_connection_status`)
- **Public Properties**: PascalCase without prefix (e.g., `DeviceName`, `IpAddress`, `ConnectionStatus`)
- **Methods**: Prefix with `Func_` (e.g., `Func_Connect()`, `Func_SendCommand()`, `Func_Log()`)
- **Enums**: Prefix with `E` (e.g., `ECommandType`, `EDeviceConnectionStatus`, `ELogPanelKeys`)

### Code Organization

- `Common/Enum/`: Enumeration types
- `Common/Logic/`: Shared utilities (NotifyPropertyChanged, RelayCommand, Singleton)
- `Model/Implementation/`: Concrete model classes
- `Model/Interface/`: Interface definitions
- `Model/Service/`: Service layer classes
- `View/Controls/`: Reusable UI controls
- `ViewModel/Controls/`: ViewModels for custom controls

### Async/Await Patterns

- Device operations should have both sync and async versions
- Use `CancellationToken` for async operations
- Async methods named with `Async` suffix (e.g., `Func_ConnectAsync()`)
- Wrap async operations in `using (CancellationTokenSource cts = new CancellationTokenSource())`

### Error Handling

- Use try-catch blocks with descriptive console output
- Log errors to appropriate log panel via `Singleton<ManagerService<ELogPanelKeys, LogPanelViewModel>>.Instance`
- Connection failures should transition through `EDeviceConnectionStatus.Connecting` to `Disconnected`
- Validate input parameters (null/empty checks) with ArgumentException

### Property Change Notification

- ViewModels inherit from `NotifyPropertyChanged`
- Call `OnPropertyChangedEventHandler(this, nameof(PropertyName))` in property setters
- Essential for WPF data binding to update UI automatically

## Testing SCPI Communication

When testing or debugging SCPI device communication:

1. Ensure device IP address and port are correctly configured
2. Device must support SCPI protocol over TCP/IP
3. Use Program Log tab to monitor connection status
4. Use Comm Log tab to see all SCPI command/response exchanges
5. Sent and Received message panels show main panel communication

Common SCPI test commands:
- `*IDN?` - Query device identification
- `*RST` - Reset device to default state
- `*CLS` - Clear status registers

## Macro System

Macros enable automated command sequences:

- Each macro has ID, Name, Description, and optional Hotkey
- Command chains executed sequentially with proper async/await
- Macro execution logs to Program Log panel
- Responses from Query commands logged to both Comm Log and Received Message panels
- Macros persist as JSON files in `./Macro/` directory
