using RDCELERP.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
//using RDCELERP.Core.App.Entities;

namespace RDCELERP.DAL.AbstractRepository
{
    /// <summary>
    /// Implementation of Repository Pattern for Entity Framework
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class AbstractRepository<TEntity> : IAbstractRepository<TEntity>
    where TEntity : class
    {
        private readonly Digi2l_DevContext _dbContext;

        public AbstractRepository(Digi2l_DevContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().AsNoTracking();
        }



        public virtual IEnumerable<TEntity> GetList(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> dbQuery = _dbContext.Set<TEntity>();

            //Apply eager loading
            //dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include(navigationProperty));

            IQueryable<TEntity> d = dbQuery.AsNoTracking().Where(where).AsQueryable();

            IEnumerable<TEntity> list = dbQuery
                .AsNoTracking()
                .Where(where)
                .ToList();

            return list;
        }

        public virtual TEntity GetSingle(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties)
        {
            IQueryable<TEntity> dbQuery = _dbContext.Set<TEntity>();

            //Apply eager loading
            //foreach (Expression<Func<TEntity, object>> navigationProperty in navigationProperties)
            //    dbQuery = dbQuery.Include<TEntity, object>(navigationProperty);

            TEntity item = dbQuery
                .AsNoTracking() //Don't track any changes for the selected item
                .FirstOrDefault(@where);

            return item;
        }

        public virtual void Create(params TEntity[] items)
        {
            foreach (TEntity item in items)
            {
                _dbContext.Entry(item).State = EntityState.Added;
            }
        }
        //Adding Multiple Records

        public virtual void AddRange(List<TEntity> entitylst)
        {
            _dbContext.Entry(entitylst).State= EntityState.Added;
        }

        public virtual void Update(params TEntity[] items)
        {
            foreach (TEntity item in items)
            {
                _dbContext.Entry(item).State = EntityState.Modified;
            }
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        //public async Task<TEntity> GetById(int id)
        //{
        //    return await _dbContext.Set<TEntity>()
        //                .AsNoTracking()
        //                .FirstOrDefaultAsync(e => e.Id == id);
        //}

        public virtual void Delete(params TEntity[] items)
        {
            foreach (TEntity item in items)
            {
                _dbContext.Entry(item).State = EntityState.Deleted;
            }
        }
        public async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        
    }
}
