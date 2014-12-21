using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Repository;
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace DAL.Repository
{
    public abstract class GenericRepository<TObject> : IDisposable where TObject : class
    {
        protected DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            // Clear up references
            this._context.Dispose();
            this._context = null;
        }

        public ICollection<TObject> GetAll(params Expression<Func<TObject, object>>[] includeItems)
        {
            return _context.Set<TObject>().IncludeManyAsDbQuery(includeItems).ToList(); ;
        }

        public async Task<ICollection<TObject>> GetAllAsync(params Expression<Func<TObject, object>>[] includeItems)
        {
            return await _context.Set<TObject>().IncludeManyAsDbQuery(includeItems).ToListAsync();
        }

        public TObject Find(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeItems)
        {
            return _context.Set<TObject>().IncludeManyAsDbQuery(includeItems).SingleOrDefault(match);
        }

        public async Task<TObject> FindAsync(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeItems)
        {
            return await _context.Set<TObject>().IncludeManyAsDbQuery(includeItems).SingleOrDefaultAsync(match);
        }

        public ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeItems)
        {
            return _context.Set<TObject>().IncludeManyAsDbQuery(includeItems).Where(match).ToList();
        }

        public async Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match, params Expression<Func<TObject, object>>[] includeItems)
        {
            return await _context.Set<TObject>().IncludeManyAsDbQuery(includeItems).Where(match).ToListAsync();
        }

        public TObject Create()
        {
            return _context.Set<TObject>().Create();
        }

        public void Add(TObject t)
        {
            _context.Entry(t).State = EntityState.Added;
            _context.Set<TObject>().Add(t);
        }
        public void AddRange(IEnumerable<TObject> entities)
        {
            //foreach (var entity in entities)
            //    _context.Entry(entity).State = EntityState.Added;

            _context.Set<TObject>().AddRange(entities);
        }

        public void BulkInsert<T>(string connection, string tableName, IList<T> list)
        {
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.BatchSize = list.Count;
                bulkCopy.DestinationTableName = tableName;

                var table = new DataTable();
                var props = TypeDescriptor.GetProperties(typeof(T))
                    //Dirty hack to make sure we only have system data types 
                    //i.e. filter out the relationships/collections
                                           .Cast<PropertyDescriptor>()
                                           .Where(propertyInfo => propertyInfo.PropertyType.Namespace.Equals("System"))
                                           .ToArray();

                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType);
                }

                var values = new object[props.Length];
                foreach (var item in list)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item);
                    }

                    table.Rows.Add(values);
                }

                bulkCopy.WriteToServer(table);
            }
        }

        // These methods are almost never worthwhile, unless customized
        // This is due to the work already having been done with formalized points of contact above
        // Find should be used to get an existing value
        public void Update(TObject t)
        {
            _context.Entry(t).State = EntityState.Modified;
        }

        public void Delete(TObject t)
        {
            _context.Entry(t).State = EntityState.Deleted;
            _context.Set<TObject>().Remove(t);
        }
        public void DeleteRange(IEnumerable<TObject> entities)
        {
            foreach (var entity in entities)
                _context.Entry(entity).State = EntityState.Deleted;

            _context.Set<TObject>().RemoveRange(entities);
        }

        public int Count()
        {
            return _context.Set<TObject>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<TObject>().CountAsync();
        }

        public int Commit()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var validationError in ex.EntityValidationErrors)
                {
                    validationError.ToString();
                }

                throw ex;
            }
        }
        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }

}
