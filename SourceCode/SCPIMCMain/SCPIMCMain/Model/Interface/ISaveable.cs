namespace SCPIMCMain.Model.Interface
{
    public interface ISaveable
    {
        public void Func_Save(string __filePath, string __jsonContent, bool __isBinary = false);
    }
}
