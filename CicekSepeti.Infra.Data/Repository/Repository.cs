using CicekSepeti.Infra.Data.Database;
using CicekSepeti.Infra.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CicekSepeti.Infra.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private DbSet<T> _objectSet;
        private bool _disposed;
        protected CicekSepetiContext context;

        protected virtual DbSet<T> Entities => _objectSet ?? (_objectSet = context.Set<T>());

        public Repository(CicekSepetiContext context)
        {
            this.context = context;
            _objectSet = this.context.Set<T>();
        }

        public IQueryable<T> ListQueryable => Entities;

        public IQueryable<T> ListQueryableNoTracking => Entities.AsNoTracking();

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Entities.Remove(entity);
            context.SaveChanges();
        }

        public void Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
                Entities.Remove(entity);
            context.SaveChanges();
        }

        public T GetById(object id)
        {
            return _objectSet.Find(id);
        }

        public IEnumerable<T> GetSql(string sql)
        {
            return Entities.FromSqlInterpolated($"{sql}");
        }

        public T Get(Expression<Func<T, bool>> filter = null)
        {
            return this._objectSet.Where(filter).FirstOrDefault();

        }
        public IQueryable<T> IncludeMany(params Expression<Func<T, object>>[] includes)
        {
            return Entities.IncludeMultiple(includes);
        }

        public T Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Entities.Add(entity);
            context.SaveChanges();

            return entity;
        }
        public void Insert(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities) // TODO performans?
                Entities.Add(entity);

            context.SaveChanges();
        }

        public T Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            Entities.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return entity;
        }

        public void Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
            //TODO
            context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                    // Dispose other managed resources.
                }
                //release unmanaged resources.
            }
            _disposed = true;
        }

        public void Detach(T entity)
        {
            context.Entry(entity).State = EntityState.Detached;
        }
    }

}
public static class Extensions
{
    public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
    {
        if (includes != null)
        {
            query = includes.Aggregate(query, (current, include) => current.Include(include));
        }
        return query;
    }
}