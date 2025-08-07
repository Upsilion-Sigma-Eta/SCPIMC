namespace SCPIMCMain.Model.Interface
{
    public interface ICommandNode
    {
        public ICommandNode Parent { get; set; }
        public ICommandNode Child { get; set; }
        public string Content { get; set; }

        public string Prefix { get; set; }
        public string Postfix { get; set; }
        public string Func_GetWholeCommand();
        public string Func_GetNodeCommand();
    }
}