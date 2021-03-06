using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Repositories
{
    public interface IRepository<KeyType,EntityType> where EntityType : class
    {
        IQueryable<EntityType> GetAll();
        EntityType GetById(KeyType id);
        EntityType GetById(KeyType id, Expression<Func<EntityType, object>> includeOption);
        EntityType Create(EntityType entity);
        void CreateList(List<EntityType> entity);
        void Delete(EntityType entity);
        void SaveChanges();
    }
}
