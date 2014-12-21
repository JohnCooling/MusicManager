using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class Repository<T> : Repository.GenericRepository<T>, IDisposable where T : class
    {
        public Repository()
            : base(new DomainModel.MusicManagerEntities())
        {

        }
    }
}
