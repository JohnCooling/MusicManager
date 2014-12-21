using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;
using System.ServiceModel;
using MusicWatcher.WCF_Service;
using Repository.Interfaces;
using DomainModel.Model;

namespace MusicWatcher
{
    public partial class MusicWatcherWindowsService : ServiceBase, IDisposable
    {
        #region Global Parameters
        private const string MUSIC_DIRECTORY = @"D:\Music\Music\iTunes\iTunes Media\Music\";
        private const string FILE_FORMAT = @"*.mp3";
        private FileSystemWatcher myWatcher = null;
        public EventLog eventLog1 = new System.Diagnostics.EventLog();
        private const NotifyFilters NOTIFICATION_FILTERS = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.Size;
        public ServiceHost serviceHost = null;
        MusicWatcherWCF myWCFService = null;
        IGenericRepository<FileInformation> _Repository;
        #endregion

        #region Main & Constructor
        public static void Main()
        {
            ServiceBase.Run(new MusicWatcherWindowsService());
        }

        public MusicWatcherWindowsService()
        {
            ServiceName = "MusicWatcherWCFService";
            //InitializeComponent();
            SetupLog();
        }
        #endregion

        #region Windows Service Events

        protected override void OnStart(string[] args)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
            }

            WriteToLog("WINDOWS SERVICE: Windows Music Watcher Service Started Correctly");
            EnableFileSystemWatcher();

            serviceHost = new ServiceHost(typeof(WCF_Service.MusicWatcherWCF));
            serviceHost.Open();
            WriteToLog("WCF SERVICE: Music Watcher WCF Service Started Correctly");

            myWCFService = new MusicWatcherWCF();
            myWCFService.MusicDirectories();
        }

        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                WriteToLog("WCF SERVICE: Music Watcher WCF Service Stopped Correctly");
                serviceHost = null;
            }

            WriteToLog("WINDOWS SERVICE: Windows Music Watcher Service Stopped Correctly");
        }

        protected override void OnPause()
        {
            WriteToLog("WINDOWS SERVICE: Windows Music Watcher Has Paused");
        }

        /* TODO - Implement Logic */
        protected void OnChanged(object source, FileSystemEventArgs args)
        {
            //switch (args.ChangeType)
            //{
            //    case WatcherChangeTypes.Created:
            //        findNewItems(source.ToList());
            //        // List.Add(item);
            //        break;
            //    case WatcherChangeTypes.Deleted:
            //        // List.Remove(item);
            //        break;
            //    case WatcherChangeTypes.Renamed:
            //        // List.Remove(oldItem);
            //        // List.Add(item);
            //        break;
            //}







        }

        /// <summary>
        /// Event Handler for renamed files - Logs file being renamed
        /// </summary>
        protected void OnRenamed(object source, RenamedEventArgs args)
        {
            WriteToLog(string.Format("FILE RENAMED: {0} Renamed To: {1}", args.OldName, args.Name));
        }

        /// <summary>
        /// Event Handler for deleted files - Logs file being removed
        /// </summary>
        protected void OnDeleted(object source, FileSystemEventArgs args)
        {
            WriteToLog(string.Format("FILE DELETED: {0} Has Been Deleted.", args.FullPath));            
        }
        #endregion

        /// <summary>
        /// Gets the paths for all files
        /// </summary>
        /// <param name="FileName">Name of the directory, including extension</param>
        public static List<string> GetFilePaths()
        {
            List<string> fileDirectories = Directory.GetFiles(MUSIC_DIRECTORY, FILE_FORMAT, SearchOption.AllDirectories).ToList();

            return fileDirectories;
        }

        public List<string> findNewItems(List<string> newList)
        {
            var oldList = GetFilePaths();

            IEnumerable<string> newItems = oldList.Except(newList);

            return newItems.ToList();
        }

        /// <summary>
        /// Initialises the FileSystemWatcher object
        /// </summary>
        protected void EnableFileSystemWatcher()
        {
            //Define FileWatch Object 
            myWatcher = new FileSystemWatcher(MUSIC_DIRECTORY, FILE_FORMAT);
            myWatcher.NotifyFilter = NOTIFICATION_FILTERS;

            myWatcher.Path = MUSIC_DIRECTORY;
            myWatcher.Changed += new FileSystemEventHandler(OnChanged);
            myWatcher.Deleted += new FileSystemEventHandler(OnDeleted);
            myWatcher.Renamed += new RenamedEventHandler(OnRenamed);

            //Enable watching
            myWatcher.EnableRaisingEvents = true;
        }

        ///// <summary>
        ///// THIS WILL BE OUR WCF SERVICE
        ///// </summary>
        ///// <param name="fileDirectories"></param>
        //protected void WriteDirectoriesToFile(List<string> fileDirectories)
        //{
        //    string fileLog = MUSIC_DIRECTORY + "fileLog.txt";

        //    if (File.Exists(fileLog))
        //    {
        //        System.IO.File.WriteAllLines(MUSIC_DIRECTORY + "fileLog.txt", fileDirectories);
        //    }
        //    else
        //    {
        //        System.IO.File.Create(MUSIC_DIRECTORY + "fileLog.txt");
        //        System.IO.File.WriteAllLines(MUSIC_DIRECTORY + "fileLog.txt", fileDirectories);
        //    }

        //}

        protected void SetupLog()
        {
            //Ensure the log is created
            if (!System.Diagnostics.EventLog.SourceExists("MusicWatcher"))
            {
                System.Diagnostics.EventLog.CreateEventSource("MusicWatcher", "MusicWatcherLog");
            }

            eventLog1.Source = "MusicWatcher";
            eventLog1.Log = "MusicWatcherLog";
        }

        /// <summary>
        /// Log a message in the Event Logger
        /// </summary>
        protected void WriteToLog(string entry)
        {
            eventLog1.WriteEntry(entry);
        }

        /// <summary>
        /// Log an error message in the event logger
        /// </summary>
        protected void WriteErrorToLog(string errorEntry)
        {
            eventLog1.WriteEntry(errorEntry, EventLogEntryType.Error);
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
    }
}
