using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System.Data.Entity;

[assembly: OwinStartupAttribute(typeof(MusicManager.Startup))]
namespace MusicManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DomainModel.MusicManagerEntities>());
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
