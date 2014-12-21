using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DomainModel
{
    public partial class MusicManagerEntities: DbContext
    {
        public MusicManagerEntities()
            : base("name=MusicManagerEntities")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<MusicManagerEntities>());
        }

        public virtual DbSet<FileMetaData> FileMeta { get; set; }
    }


}