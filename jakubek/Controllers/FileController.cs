using jakubek.Exceptions;
using jakubek.Models;
using jakubek.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost]
        public ActionResult Upload([FromForm] IFormFile file, [FromForm] string jsonString)
        {
            if (file is null || file.Length <= 0)
                throw new BadRequestException("Nie znaleziono wymaganego pliku");

            string rootPath = Directory.GetCurrentDirectory();
            string fileName = file.FileName;
            string fullPath = $"{rootPath}/PrivateFiles/{fileName}";
            if (System.IO.File.Exists(fullPath))
                return Ok(new { message = "Plik o takiej nazwie już istnieje" });
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            FileViewModel fileModel = JsonConvert.DeserializeObject<FileViewModel>(jsonString);
            _fileService.CreateFile(fileModel);
            return Ok(new { message = "Udało się poprawnie utworzyć plik"});
        }
    }
}
