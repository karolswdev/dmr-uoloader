using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UOLoader.Server.Data;
using UOLoader.Server.Entities;
using UOLoader.Server.Helpers;

namespace UOLoader.Server.Services
{
    /// <summary>
    /// Used to scan files for updates. Handy when changing Docker hosts and populating internal database
    /// </summary>
    public class ScannerService : BackgroundService
    {
        private readonly UoLoaderContext _dbContext;
        private readonly ILogger<ScannerService> _logger;

        public ScannerService(UoLoaderContext dbContext, ILogger<ScannerService> logger) {
            _dbContext = dbContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            if (Directory.Exists(Constants.DataFilesDirectory))
            {
                foreach (var file in Directory.EnumerateFiles(Constants.DataFilesDirectory))
                {
                    var parts = UpdateFilePartInfo.GetFromPath(file);
                    if (parts != null)
                    {
                        if (!_dbContext.UpdateFiles.Any(f => f.Name == parts.Name && f.Revision == parts.Revision))
                        {

                            _logger.LogInformation("Adding scanned files into database {fileName}, {fileRevision}", parts.Name, parts.Revision);
                            _dbContext.UpdateFiles.Add(new UpdateFile()
                            {
                                Name = parts.Name,
                                RelativeUri = parts.RelativeUri,
                                Revision = parts.Revision,
                                SizeInKb = parts.SizeInKb
                            });
                        }
                    }

                    _logger.LogInformation("Adding scanned files into database {fileName}, {fileRevision}", parts.Name, parts.Revision);
                    await _dbContext.SaveChangesAsync(stoppingToken);
                }
            }
            else
            {
                _logger.LogCritical("No data files directory found!");
            }
        }
    }
}
