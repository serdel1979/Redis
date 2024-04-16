using System.Linq.Expressions;

namespace Redis.Repository
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> GetAllFilter(Expression<Func<TEntity,bool>>?filter=null); 
        Task Create(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);

        Task SaveAll();
    }
}
