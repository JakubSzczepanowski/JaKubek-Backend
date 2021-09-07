using jakubek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Services.Interfaces
{
    public interface IFileService
    {
        void CreateFile(FileViewModel file);
        PagedResult<FileListViewModel> GetExistingFiles(BaseQuery query);
        Tuple<string,string> GetFileById(int id);
        PagedResult<FileListViewModel> GetUserFiles(BaseQuery query);
        void UpdateFile(int id, FileUpdateViewModel fileUpdateViewModel);
        void DeleteFile(int id);
    }
}
