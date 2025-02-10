using RDCELERP.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RDCELERP.DAL.AbstractRepository
{
    public interface IAbstractRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        //Task<TEntity> GetById(int id);

        IEnumerable<TEntity> GetList(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties);
        TEntity GetSingle(Func<TEntity, bool> where, params Expression<Func<TEntity, object>>[] navigationProperties);

        void Create(params TEntity[] items);

        void Update(params TEntity[] items);

        int SaveChanges();

        Task CreateAsync(TEntity entity);

        Task UpdateAsync(int id, TEntity entity);

        void Delete(params TEntity[] items);

        //Task Delete(int id);
        Task DeleteAsync(TEntity entity);
    }
}

