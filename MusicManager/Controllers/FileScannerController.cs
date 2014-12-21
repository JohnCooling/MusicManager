using DomainModel;
using MusicManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DAL.Repository;
using BLL.Helpers.FileScanner;
using System.IO;

namespace MusicManager.Controllers
{
    public class FileScannerController : Controller
    {
        private MusicManagerEntities db = new MusicManagerEntities();  
        
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Scan()
        {
            List<FileMetaData> fileDirectories = new List<FileMetaData>();
            string connectionString = db.Database.Connection.ConnectionString;

            using (FilePathScanner scanner = new FilePathScanner())
            {
                fileDirectories = scanner.GetFilePaths();
            }

           TagLib.File file = null;

            List<FileMetaData> completeMetaDataList = new List<FileMetaData>();
            
            foreach (var item in fileDirectories)
            {
                file = TagLib.File.Create(item.FileLocation);
                FileInfo fileSize = new FileInfo(item.FileLocation);

                var metaData = new FileMetaData
                {
                    Album = file.Tag.Album,
                    //AlbumArt = file.TagHandler.Picture.
                    AlbumArtist = file.Tag.FirstAlbumArtist,
                    Artist = file.Tag.FirstPerformer,
                    BitRate = file.Properties.AudioBitrate.ToString(),
                    BPM = file.Tag.BeatsPerMinute.ToString(),
                    Comments = file.Tag.Comment,
                    Composer = file.Tag.FirstComposer,
                    Genre = file.Tag.FirstGenre,
                    Kind = file.MimeType,
                    Name = file.Tag.Title,
                    SampleRate = file.Properties.AudioSampleRate.ToString(),
                    Size = fileSize.Length.ToString(),
                    TotalTime = file.Properties.Duration.ToString(),
                    TrackNumber = file.Tag.Track.ToString(),
                    Year = file.Tag.Year.ToString(),
                    DateAdded = System.DateTime.Now,
                    IsDeleted = false,
                    GUID = System.Guid.NewGuid(),
                    FileLocation = item.FileLocation,
                };
                completeMetaDataList.Add(metaData);
            }
            using (Repository<FileMetaData> rep = new Repository<FileMetaData>())
            {
                rep.BulkInsert(connectionString, "FileMetaDatas", completeMetaDataList);

                rep.Commit();
            }

            return View();
        }
    }
}
