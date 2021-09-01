using jakubek.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Models
{
    public class FileViewModel
    {
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
