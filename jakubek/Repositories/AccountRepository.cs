using jakubek.Entities;
using jakubek.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Repositories
{
    public class AccountRepository : Repository<int,User>, IAccountRepository
    {
        public AccountRepository(BaseContext baseContext) : base(baseContext) { }

        public User GetUserByLogin(string login, Expression<Func<User, object>> includeOption) => DbSet.Include(includeOption).FirstOrDefault(e => e.Login.Equals(login));
    }
}
