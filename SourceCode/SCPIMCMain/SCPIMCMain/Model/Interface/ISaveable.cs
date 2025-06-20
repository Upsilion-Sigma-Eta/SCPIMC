using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCPIMCMain.Model.Interface
{
    public interface ISaveable
    {
        public void Save(string filePath, string jsonContent, bool isBinary = false);
    }
}
