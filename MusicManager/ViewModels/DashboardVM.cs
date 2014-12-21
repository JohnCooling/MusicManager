using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicManager.ViewModels
{
    public class DashboardVM
    {
        public int FileCount { get; set; }

        public List<FileMetaData> RecentFiles { get; set; }
    }
}