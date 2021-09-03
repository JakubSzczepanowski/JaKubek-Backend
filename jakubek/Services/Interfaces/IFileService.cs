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
        List<FileListViewModel> GetExistingFiles();
        Tuple<string,string> GetFileById(int id);
        void UpdateFile(int id, FileUpdateViewModel fileUpdateViewModel);
        void DeleteFile(int id);
    }
}
