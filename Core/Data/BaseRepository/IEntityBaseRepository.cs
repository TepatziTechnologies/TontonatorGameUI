using System;
using System.Threading.Tasks;

namespace TontonatorGameUI.Core.Data.BaseRepository
{
    public interface IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        T Add(T entity);
        Task Delete(T entity);
        Task Update(T entity);
        T Read(string field, string query);
    }
}