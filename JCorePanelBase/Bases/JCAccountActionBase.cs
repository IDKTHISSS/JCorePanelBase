using System.Collections.Generic;

namespace JCorePanelBase
{
    public class JCAccountActionBase
    {
        public List<JCAction> ActionList = new List<JCAction>();
        public void LoadActions(JCSteamAccount CurrectAccount)
        {
            if (CurrectAccount == null) return;
        }
        public List<JCAction> GetActions()
        {
            return ActionList;
        }
        protected void AddAction(JCAction Action)
        {
            ActionList.Add(Action);
        }
    }
}
