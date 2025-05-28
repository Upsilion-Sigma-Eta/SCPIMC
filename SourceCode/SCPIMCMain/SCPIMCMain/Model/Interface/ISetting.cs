using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPIMCMain.Model.Interface
{
    public interface ISetting<SettingValueType> : ILoadable, ISaveable
    {
        public SettingValueType Value { get; set; }

        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string Description { get; set; }
        public void ResetSettings();
        public void ValidateSettings();
        public event EventHandler SettingsChanged;
    }
}
