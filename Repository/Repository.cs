using Microsoft.EntityFrameworkCore;
using Redis.Model;
using System.Linq.Expressions;

namespace Redis.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ContextWork _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ContextWork context)
        {
            this._context = context;
            _dbSet = _context.Set<TEntity>();

        }
        public async Task Create(TEntity entity)
        {
            _context.Add(entity);
        }

        public async Task Delete(TEntity entity)
        {
            _dbSet.Update(entity);
        }


        public IQueryable<TEntity> GetAllFilter(Expression<Func<TEntity, bool>>? filter = null)
        {
            IQueryable<TEntity> finds = filter == null ? _dbSet : _dbSet.Where(filter);
            return finds;
        }

        public async Task Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task SaveAll()
        {
            await _context.SaveChangesAsync();
        }
    }
}
