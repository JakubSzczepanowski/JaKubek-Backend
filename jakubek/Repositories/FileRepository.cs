using jakubek.Entities;
using jakubek.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Repositories
{
    public class FileRepository : Repository<int,Entities.File>, IFileRepository
    {
        public FileRepository(BaseContext baseContext) : base(baseContext) { }
    }
}
