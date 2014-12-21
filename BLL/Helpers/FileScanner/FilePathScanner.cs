using DomainModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BLL.Helpers.FileScanner
{
    public class FilePathScanner : IDisposable
    {
        public List<string> _fileDirectories;
        public List<FileMetaData> _fileList;
        public FileMetaData _fileInfo;
        string _MusicDirectory = @"C:\Users\Johnathan\Desktop\+44";
        string _FileFormat = @"*.mp3";
        private bool _disposed;
        public int _directoriesFound;

        public List<FileMetaData> GetFilePaths()
        {
            _fileDirectories = Directory.GetFiles(_MusicDirectory, _FileFormat, SearchOption.AllDirectories).ToList();
            _fileList = new List<FileMetaData>();

            foreach (var item in _fileDirectories)
            {
                _fileInfo = new FileMetaData
                {
                    FileLocation = item,                    
                };

                _fileList.Add(_fileInfo);

            }

            _directoriesFound = _fileList.Count();

            return _fileList;
        }

        //public List<string> FindNewDirectories(List<string> oldList)
        //{
        //    var newList = GetFilePaths();

        //    //IEnumerable<string> newItems = newList.Except(oldList);

        //    return newItems.ToList();
        //}




        /// <summary>
        /// Implement IDisposable cleanup
        /// </summary>
        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposal
        /// </summary>
        /// <param name="disposing">boolean - is object being disposed?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {

            }
            _disposed = true;
        }
    }
}