using jakubek.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public ActionResult Upload([FromBody] FileViewModel file)
        {
            if (file is null || file.FileContent.Length <= 0)
                return BadRequest();

            string rootPath = Directory.GetCurrentDirectory();
            string fileName = file.FileContent.FileName;
            string fullPath = $"{rootPath}/PrivateFiles/{fileName}";
            using(var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.FileContent.CopyTo(stream);
            }
            return Ok(new { message = "Udało się poprawnie utworzyć plik"});
        }
    }
}
