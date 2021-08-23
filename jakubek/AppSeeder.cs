using jakubek.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek
{
    public class AppSeeder
    {
        private readonly BaseContext _baseContext;
        public AppSeeder(BaseContext baseContext)
        {
            _baseContext = baseContext;
        }
        public void Seed()
        {
            if(_baseContext.Database.CanConnect())
            {
                if(!_baseContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _baseContext.Roles.AddRange(roles);
                    _baseContext.SaveChanges();
                }
            }
        }
        
        private IEnumerable<Role> GetRoles()
        {
            return new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };
        }
    }
}
