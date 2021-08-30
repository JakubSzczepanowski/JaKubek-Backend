using jakubek.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<int,User>
    {
        User GetUserByLogin(string login, Expression<Func<User, object>> includeOption);
    }
}
