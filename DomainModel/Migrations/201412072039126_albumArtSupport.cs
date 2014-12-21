namespace DomainModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class albumArtSupport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileMetaDatas", "AlbumArt", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileMetaDatas", "AlbumArt");
        }
    }
}
