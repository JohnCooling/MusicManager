using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel
{
    public partial class FileMetaData
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string AlbumArtist { get; set; }
        public string Composer { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public string Kind { get; set; }
        public string Size { get; set; }
        public string TotalTime { get; set; }
        public string TrackNumber { get; set; }
        public string Year { get; set; }
        public string BPM { get; set; }
        public string BitRate { get; set; }
        public string SampleRate { get; set; }
        public string Comments { get; set; }
        public string FileLocation { get; set; }
        public System.DateTime DateAdded { get; set; }
        public Nullable<System.DateTime> DateRemoved { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public bool IsDeleted { get; set; }
        public System.Guid GUID { get; set; }
        public byte AlbumArt { get; set; }
    }
}