using SCPIMCMain.Common.Enum;

namespace SCPIMCMain.Model.Interface
{
    public interface IMacro : ILoadable, ISaveable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HotkeyKeyCode { get; set; }
        public List<(ECommandType, string)> CommandChain { get; set; }

        public void Func_ExecuteMacro();
        public void Func_AddCommand(ECommandType __commandType, string __command);
        public void Func_RemoveCommand(int __index);
        public void Func_ClearCommands();
        public void Func_ResetMacro();
        public void Func_ValidateMacro();
        public void Func_ReloadMacro();
        public void Func_DeleteMacro();
    }
}
