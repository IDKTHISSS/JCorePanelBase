using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCorePanelBase.Structures
{
    public struct JCEventProperty
    {
        public string Name;
        public string Value;
        public JCEventProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
