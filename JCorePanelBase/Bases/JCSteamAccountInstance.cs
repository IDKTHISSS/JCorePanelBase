using SteamKit2.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JCorePanelBase
{
    public delegate void AccountStatusChangedEventHandler(string newStatus);
    public delegate void AccountIsInWorkChangedEventHandler(bool newStatus);
    public delegate void AccountErrorChangedEventHandler(bool newStatus);
    public delegate void AccountWorkStatusChangedEventHandler(string newStatus);
    public class JCSteamAccountInstance
    {
        public JCSteamAccount AccountInfo;
        public SteamAccountCache AccountCache;

        private string _status;
        private bool _error;
        private bool _IsInWork;
        private string _WorkStatus;



        public event AccountStatusChangedEventHandler StatusChangedHandler;
        public event AccountErrorChangedEventHandler ErrorChangedHandler;
        public event AccountIsInWorkChangedEventHandler IsInWorkChangedHandler;
        public event AccountWorkStatusChangedEventHandler WorkStatusChangedHandler;

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
        public string Status
        {
            get { return _status; }
            set
            {
                // Проверяем, изменилось ли значение
                if (_status != value)
                {
                    _status = value;

                    // Генерируем событие, уведомляя об изменении значения
                    OnStatusChanged(_status);
                }
            }
        }
        protected virtual void OnStatusChanged(string newValue)
        {
            StatusChangedHandler?.Invoke(newValue);
        }
        protected virtual void OnIsInWorkChanged(bool newValue)
        {
            IsInWorkChangedHandler?.Invoke(newValue);
        }
        protected virtual void OnErrorChanged(bool newValue)
        {
            ErrorChangedHandler?.Invoke(newValue);
        }
        protected virtual void OnWorkStatusChanged(string newValue)
        {
            WorkStatusChangedHandler?.Invoke(newValue);
        }
        public void SetStatus(string NewStatus)
        {
            Status = NewStatus;
        }
        public void SetStatusH(string NewStatus)
        {
            Status = "#" + NewStatus;
        }
        public void SetInWork(bool NewInWork)
        {
            IsInWork = NewInWork;
        }
        public void SetWorkStatus(string NewWorkStatus)
        {
            WorkStatus = NewWorkStatus;
        }
        public void SetError(string Messege)
        {
            SetWorkStatus(Messege);
            Error = true;
            Thread.Sleep(5000);
            Error = false;
            IsInWork = false;
        }
    }
}
