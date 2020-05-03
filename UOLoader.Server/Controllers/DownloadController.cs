using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UOLoader.Server.Data;
using UOLoader.Server.Entities;

namespace UOLoader.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly UoLoaderContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<DownloadController> _logger;

        public DownloadController(UoLoaderContext context, IHttpContextAccessor accessor, ILogger<DownloadController> logger) {
            _context = context;
            _accessor = accessor;
            _logger = logger;
        }
        [HttpGet("uo")]
        public IActionResult DownloadUltima() {

            var log = new Log();
            log.HostName = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            log.When = DateTime.Now;
            log.LogText = "Full ultima download endpoint hit";
            log.IpAddress = log.HostName;
            _context.Logs.Add(log);
            _context.SaveChanges();

            _logger.LogInformation("Ultima full download being requested");

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

            var log = new Log();
            log.HostName = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            log.When = DateTime.Now;
            log.LogText = "File endpoint hit";
            log.IpAddress = log.HostName;
            _context.Logs.Add(log);
            _context.SaveChanges();

            _logger.LogInformation("Ultima file being requested");

            var fileFs = Path.Combine(Constants.DataFilesDirectory, file.RelativeUri);

            if (!System.IO.File.Exists(fileFs)) {
                return NotFound("File not found");
            }

            var fileContents = new FileStream(fileFs, FileMode.Open);
            return File(fileContents, "application/octet-stream");

        }
    }
}
