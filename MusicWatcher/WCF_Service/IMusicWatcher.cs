using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MusicWatcher.WCF_Service
{
    [ServiceContract(Namespace = "http://MusicWatcher.WCF_Service")]
    public interface IMusicWatcher
    {
        [OperationContract]
        List<string> MusicDirectories();
    }

}
