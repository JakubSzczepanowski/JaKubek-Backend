using jakubek.Authorization;
using jakubek.Exceptions;
using jakubek.Models;
using jakubek.Repositories.Interfaces;
using jakubek.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Services
{
    public class FileService : IFileService
    {
        private readonly IUserContextService _userContextService;
        private readonly IFileRepository _fileRepository;
        private readonly IAuthorizationService _authorizationService;
        public FileService(IUserContextService userContextService, IFileRepository fileRepository, IAuthorizationService authorizationService)
        {
            _userContextService = userContextService;
            _fileRepository = fileRepository;
            _authorizationService = authorizationService;
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

        public PagedResult<FileListViewModel> GetExistingFiles(BaseQuery query)
        {
            var baseQuery = _fileRepository.GetAll().Where(r => query.SearchPhrase == null || (r.Name.ToLower().Contains(query.SearchPhrase.ToLower()) || r.Description.ToLower().Contains(query.SearchPhrase.ToLower())));

            if(!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Entities.File, object>>>()
                {
                    { nameof(Entities.File.Name), r => r.Name},
                    { nameof(Entities.File.Description), r => r.Description}
                };
                var selectedColumn = columnSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var listFiles = baseQuery
                .Skip(query.PageSize*(query.PageNumber-1))
                .Take(query.PageSize)
                .Select(e => new FileListViewModel
                {
                    Id = e.Id,
                    FileName = e.FileName,
                    Name = e.Name,
                    Description = e.Description,
                    UserName = e.User.Login
                }).ToList();

            var result = new PagedResult<FileListViewModel>(listFiles, baseQuery.Count(), query.PageSize, query.PageNumber);

            return result;
        }

        public PagedResult<FileListViewModel> GetUserFiles(BaseQuery query)
        {
            int userId = _userContextService.GetUserId;
            var baseQuery = _fileRepository.GetAll().Where(c => c.UserId == userId).Where(r => query.SearchPhrase == null || (r.Name.ToLower().Contains(query.SearchPhrase.ToLower()) || r.Description.ToLower().Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnSelectors = new Dictionary<string, Expression<Func<Entities.File, object>>>()
                {
                    { nameof(Entities.File.Name), r => r.Name},
                    { nameof(Entities.File.Description), r => r.Description}
                };
                var selectedColumn = columnSelectors[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var listFiles = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .Select(e => new FileListViewModel
                {
                    Id = e.Id,
                    FileName = e.FileName,
                    Name = e.Name,
                    Description = e.Description,
                    UserName = e.User.Login
                }).ToList();

            var result = new PagedResult<FileListViewModel>(listFiles, baseQuery.Count(), query.PageSize, query.PageNumber);

            return result;
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

        public void UpdateFile(int id, FileUpdateViewModel fileUpdateViewModel)
        {
            var file = _fileRepository.GetById(id);
            if (file is null)
                throw new NotFoundException("Nie znaleziono pliku do edycji");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, file, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
                throw new ForbidException("Nie masz uprawnień do edytowania tego pliku");

            file.Name = fileUpdateViewModel.Name;
            file.Description = fileUpdateViewModel.Description;

            _fileRepository.SaveChanges();
        }

        public void DeleteFile(int id)
        {
            var fileRecord = _fileRepository.GetById(id);

            if (fileRecord is null)
                throw new NotFoundException("Nie znaleziono takiego pliku do usunięcia");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, fileRecord, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
                throw new ForbidException("Nie masz uprawnień do usunięcia tego pliku");

            string fileName = fileRecord.FileName;
            string rootPath = Directory.GetCurrentDirectory();
            string filePath = $"{rootPath}/PrivateFiles/{fileName}";

            System.IO.File.Delete(filePath);
            _fileRepository.Delete(fileRecord);
            _fileRepository.SaveChanges();
        }
    }
}
