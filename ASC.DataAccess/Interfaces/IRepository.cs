using ASC.Model.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.DataAccess.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> FindAsync(string partitionKey, string rowKey);
        Task<IEnumerable<T>> FindAllAsync();
        Task<IEnumerable<T>> FindAllByPartitionKeyAsync(string partitionKey);
    }
}
