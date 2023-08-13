using JCorePanelBase.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JCorePanelBase
{
    public delegate void TaskErrorChangedEventHandler(bool newStatus);
    public delegate void TaskWorkStatusChangedEventHandler(string newStatus);
    public class JCEventBase
    {
        public string Name;
        public string Description;
        public delegate void MyStaticFunctionDelegate(string message);
        public List<JCEventProperty> Properties = new List<JCEventProperty>();

        private bool _error;
        private string _WorkStatus;

        public event TaskErrorChangedEventHandler ErrorChangedHandler;
        public event TaskWorkStatusChangedEventHandler WorkStatusChangedHandler;

        public string WorkStatus
        {
            get { return _WorkStatus; }
            set
            {
                // Проверяем, изменилось ли значение
                if (_WorkStatus != value)
                {
                    _WorkStatus = value;

                    // Генерируем событие, уведомляя об изменении значения
                    OnWorkStatusChanged(_WorkStatus);
                }
            }
        }
        public bool Error
        {
            get { return _error; }
            set
            {
                // Проверяем, изменилось ли значение
                if (_error != value)
                {
                    _error = value;

                    // Генерируем событие, уведомляя об изменении значения
                    OnErrorChanged(_error);
                }
            }
        }

        protected virtual void OnErrorChanged(bool newValue)
        {
            ErrorChangedHandler?.Invoke(newValue);
        }
        protected virtual void OnWorkStatusChanged(string newValue)
        {
            WorkStatusChangedHandler?.Invoke(newValue);
        }

        public void SetStatus(string Messege)
        {
            WorkStatus = Messege;
        }
        public void SetError(string Messege)
        {
            SetStatus(Messege);
            Error = true;
            Thread.Sleep(5000);
            Error = false;
        }

        public string GetPropertie(string propertieName)
        {
            foreach (JCEventProperty property in Properties)
            {
                if(property.Name == propertieName) return property.Value;
            }
            return null;
        }
        public JCEventBase(List<JCEventProperty> PropertiesList)
        {
            LoadProperties(PropertiesList);
        }
        public JCEventBase()
        {
        }
        public void AddProperty(JCEventProperty property)
        {
            if(GetPropertie(property.Name) == null)
            {
                Properties.Add(property);
            }
        }
        public void LoadProperties(List<JCEventProperty> PropertiesList)
        {
            if (PropertiesList == null) return;
            Properties.AddRange(PropertiesList);
        }
        public void EventBody(List<JCSteamAccountInstance> Accounts) { }

    }
}
