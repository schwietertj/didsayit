using System.Linq;
using System.Threading.Tasks;
using DidSayItModels;

namespace DidSayIt.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAll(bool includeInactive = false);

        Task<TEntity> GetById(long id);

        Task<TEntity> Save(TEntity entity);

        Task Delete(long id);
    }
}
