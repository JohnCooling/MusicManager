using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicWatcher.WCF_Service
{
    public class MusicWatcherWCF : IMusicWatcher
    {
        public List<string> MusicDirectories()
        {
            return MusicWatcherWindowsService.GetFilePaths();
        }
    }
}
