namespace SCPIMCMain.Model.Interface
{
    public interface ISetting<TSettingValueType> : ILoadable, ISaveable
    {
        public TSettingValueType Value { get; set; }

        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string Description { get; set; }
        public void Func_ResetSettings();
        public void Func_ValidateSettings();
        public event EventHandler ESettingsChanged;
    }
}
