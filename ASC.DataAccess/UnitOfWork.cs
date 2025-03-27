using ASC.DataAccess.Interfaces;
using ASC.Model.BaseTypes;
using Microsoft.EntityFrameworkCore;


namespace ASC.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private Dictionary<string, object> _repositories;
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext) 
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _repositories = new Dictionary<string, object>();
        }


        public int CommitTransaction()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbContext);
                _repositories[type] = repositoryInstance!;
            }
            return (IRepository<T>)_repositories[type];
        }
    }
}
