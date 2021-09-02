using jakubek.Exceptions;
using jakubek.Models;
using jakubek.Repositories.Interfaces;
using jakubek.Services.Interfaces;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Services
{
    public class FileService : IFileService
    {
        private readonly IUserContextService _userContextService;
        private readonly IFileRepository _fileRepository;
        public FileService(IUserContextService userContextService, IFileRepository fileRepository)
        {
            _userContextService = userContextService;
            _fileRepository = fileRepository;
        }
        public void CreateFile(FileViewModel file)
        {
            int userId = _userContextService.GetUserId;

            var newFile = new Entities.File()
            {
                Name = file.Name,
                FileName = file.FileName,
                Description = file.Description,
                UserId = userId
            };

            _fileRepository.Create(newFile);
            _fileRepository.SaveChanges();
        }

        public List<FileListViewModel> GetExistingFiles()
        {
            var listFiles = _fileRepository.GetAll().Select(e => new FileListViewModel{ 
            Id = e.Id,
            FileName = e.FileName,
            Name = e.Name,
            Description = e.Description
            }).ToList();

            return listFiles;
        }

        public Tuple<string,string> GetFileById(int id)
        {
            string fileName = _fileRepository.GetById(id).FileName;
            string rootPath = Directory.GetCurrentDirectory();
            string filePath = $"{rootPath}/PrivateFiles/{fileName}";

            if (!System.IO.File.Exists(filePath))
                throw new NotFoundException("Nie znaleziono pliku");

            return Tuple.Create(filePath,fileName);
        }
    }
}
