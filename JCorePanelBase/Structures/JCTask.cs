using JCorePanelBase.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCorePanelBase
{
    public struct JCTask
    {
        public string Name;
        public string PluginName;
        public string TaskName;
        public List<JCEventProperty> PropertiesList;
        public JCTask(string name, string pluginName, string taskName, List<JCEventProperty> properties)
        {
            Name = name;
            PluginName = pluginName;
            TaskName = taskName;
            PropertiesList = properties;
        }
    }
}
