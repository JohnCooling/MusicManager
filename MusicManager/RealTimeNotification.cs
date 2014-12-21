using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading;
using BLL.Helpers.FileScanner;
using Microsoft.AspNet.SignalR.Hubs;

namespace MusicManager
{
    [HubName("realTimeNotification")] 
    public class RealTimeNotification : Hub
    {
        int recordsToBeProcessed;

        public void DoLongOperation()
        {
            using (FilePathScanner scanner = new FilePathScanner())
            {
               recordsToBeProcessed = scanner.GetFilePaths().Count();
            }

            for (int record = 0; record <= recordsToBeProcessed; record++)
            {
                if (ShouldNotifyClient(record))
                {
                    Clients.Caller.sendMessage(string.Format
                    ("Processing item {0} of {1}", record, recordsToBeProcessed));
                    Thread.Sleep(1);
                }
            }
        }

        private static bool ShouldNotifyClient(int record)
        {
            return record % 10 == 0;
        }
    }
}