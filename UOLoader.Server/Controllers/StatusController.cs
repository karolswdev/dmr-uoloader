using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UOLoader.Contract;
using UOLoader.Server.Data;
using UOLoader.Server.Entities;
using UOLoader.Server.Helpers;

namespace UOLoader.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController
    {
        private readonly UoLoaderContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger<StatusController> _logger;

        public StatusController(UoLoaderContext context, IHttpContextAccessor accessor, ILogger<StatusController> logger) {
            _context = context;
            _accessor = accessor;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index() {

            var log = new Log();
            log.HostName = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            log.When = DateTime.Now;
            log.LogText = "Status endpoint hit";
            log.IpAddress = log.HostName;
            _context.Logs.Add(log);
            _context.SaveChanges();

            _logger.LogInformation("Status being requested");

            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            if (String.IsNullOrEmpty(config["SECRET_PASSWORD"])) {
                return new BadRequestResult();
            }

            var settings = _context.Settings.FirstOrDefault();

            if (settings == null) {
                var newSettings = new Settings() {
                    MessageOfTheDay = "Ultima DMR wita!",
                    RelativeUoUri = "uo.zip",
                    UoSizeInKb = 800000
                };
                _context.Settings.Add(newSettings);
                await _context.SaveChangesAsync();
                settings = newSettings;
            }

            var responseMessage = new UpdatePayload() {
                FullUltimaOnlineDownloadPath = $"{config["URL_BASE"]}/api/download/uo",
                FullUltimaOnlineDownloadWeightInKb = settings.UoSizeInKb,
                MessageOfTheDay = settings.MessageOfTheDay
            };

            var files = _context.UpdateFiles.Select(p => new UpdateFileInfo() {
                AssociatedMessage = p.AssociatedMessage, Name = p.Name, Revision = p.Revision, SizeInKb = p.SizeInKb,
                RequiresUnzip = p.RequiresUnzip, Uri = UrlHelper.GetFullUrl(p.RelativeUri, p.Revision), TargetName = p.TargetName
            });

            responseMessage.UpdateFiles = files.ToList();
            return new JsonResult(responseMessage);

        }
    }
}
