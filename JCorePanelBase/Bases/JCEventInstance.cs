using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JCorePanelBase
{
    
    public delegate void TaskIsInWorkChangedEventHandler(bool newStatus);

    public class JCEventInstance
    {
       
        private bool _IsInWork;


        public event TaskIsInWorkChangedEventHandler IsInWorkChangedHandler;
        

        public bool IsInWork
        {
            get { return _IsInWork; }
            set
            {
                // Проверяем, изменилось ли значение
                if (_IsInWork != value)
                {
                    _IsInWork = value;

                    // Генерируем событие, уведомляя об изменении значения
                    OnIsInWorkChanged(_IsInWork);
                }
            }
        }
       
        protected virtual void OnIsInWorkChanged(bool newValue)
        {
            IsInWorkChangedHandler?.Invoke(newValue);
        }
        
        
        public void SetIsInWork(bool newValue)
        {
            IsInWork = newValue;
        }
       
    }
}
