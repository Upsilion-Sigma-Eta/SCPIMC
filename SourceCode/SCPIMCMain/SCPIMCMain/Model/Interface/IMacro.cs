using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void ExecuteMacro();
        public void AddCommand(ECommandType commandType, string command);
        public void RemoveCommand(int index);
        public void ClearCommands();
        public void ResetMacro();
        public void ValidateMacro();
        public void UpdateMacro();
        public void DeleteMacro();
    }
}
