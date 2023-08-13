using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCorePanelBase
{
    public struct JCSteamGuardResponse
    {
        public bool success;
        public int PID;
        public string message;
        public JCSteamGuardResponse(bool Success, string Message, int pid)
        {
            success = Success;
            message = Message;
            PID = pid;
        }
    }
}
