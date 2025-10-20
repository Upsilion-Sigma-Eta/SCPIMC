using SCPIMCMain.Common.Enum;

namespace SCPIMCMain.Model.Implementation
{
    /// <summary>
    /// Data Transfer Object for MacroModel serialization
    /// </summary>
    public class MacroDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HotkeyKeyCode { get; set; }
        public List<CommandDto> CommandChain { get; set; }

        public MacroDto()
        {
            CommandChain = new List<CommandDto>();
        }
    }

    /// <summary>
    /// Data Transfer Object for Command tuple
    /// </summary>
    public class CommandDto
    {
        public ECommandType CommandType { get; set; }
        public string CommandText { get; set; }

        public CommandDto()
        {
        }

        public CommandDto(ECommandType __commandType, string __commandText)
        {
            CommandType = __commandType;
            CommandText = __commandText;
        }
    }
}
