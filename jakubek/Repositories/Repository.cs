using jakubek.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Repositories
{
    public class Repository<KeyType,EntityType> : IRepository<KeyType,EntityType> where EntityType : class, IEntity<KeyType>
    {
        protected BaseContext DbContext { get; }
        protected DbSet<EntityType> DbSet { get; }
        public Repository(BaseContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<EntityType>();
        }
        public virtual EntityType GetById(KeyType id) => DbSet.FirstOrDefault(e => e.Id.Equals(id));
        public virtual EntityType GetById(KeyType id, Expression<Func<EntityType, object>> includeOption) => DbSet.Include(includeOption).FirstOrDefault(e => e.Id.Equals(id));
        public virtual EntityType Create(EntityType entity) => DbSet.Add(entity).Entity;
        public virtual void CreateList(List<EntityType> entity) => DbSet.AddRange(entity);
        public void SaveChanges() => DbContext.SaveChanges();
    }
}
