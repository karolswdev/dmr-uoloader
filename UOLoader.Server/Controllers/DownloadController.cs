using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UOLoader.Server.Data;

namespace UOLoader.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly UoLoaderContext _context;

        public DownloadController(UoLoaderContext context) {
            _context = context;
        }
        [HttpGet("uo")]
        public IActionResult DownloadUltima() {

            var uo = Path.Combine(Constants.DataDirectory, "uo.zip");
            if (!System.IO.File.Exists(uo)) {
                return BadRequest("No Ultima Online found!");
            }

            var fileContents = new FileStream(uo, FileMode.Open);
            return File(fileContents, "application/octet-stream");
        }

        [HttpGet("file/{name}/{revision}")]
        public IActionResult DownloadFile(string name, int revision) {

            var file = _context.UpdateFiles.FirstOrDefault(f => f.Name == name && f.Revision == revision);

            if (file == null) {
                return NotFound("File not found");
            }

            var fileFs = Path.Combine(Constants.DataFilesDirectory, file.RelativeUri);

            if (!System.IO.File.Exists(fileFs)) {
                return NotFound("File not found");
            }

            var fileContents = new FileStream(fileFs, FileMode.Open);
            return File(fileContents, "application/octet-stream");

        }
    }
}
