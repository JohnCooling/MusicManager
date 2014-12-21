namespace DomainModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialModelCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileMetaDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Artist = c.String(),
                        AlbumArtist = c.String(),
                        Composer = c.String(),
                        Album = c.String(),
                        Genre = c.String(),
                        Kind = c.String(),
                        Size = c.String(),
                        TotalTime = c.String(),
                        TrackNumber = c.String(),
                        Year = c.String(),
                        BPM = c.String(),
                        BitRate = c.String(),
                        SampleRate = c.String(),
                        Comments = c.String(),
                        FileLocation = c.String(),
                        DateAdded = c.DateTime(nullable: false),
                        DateRemoved = c.DateTime(),
                        DateModified = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        GUID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FileMetaDatas");
        }
    }
}
