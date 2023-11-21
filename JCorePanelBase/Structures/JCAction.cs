using System;
using System.Threading.Tasks;

namespace JCorePanelBase
{
    public struct JCAction
    {
        public string Name;
        public string FriendlyName;
        public Func<JCSteamAccountInstance, Task> ActionFunction;
        public string SubMenuName;

        public JCAction(string name, string friendlyName, Func<JCSteamAccountInstance, Task> actionFunction, string subMenu = null)
        {
            Name = name;
            FriendlyName = friendlyName;
            ActionFunction = actionFunction;
            SubMenuName = subMenu;
        }
    }
}
