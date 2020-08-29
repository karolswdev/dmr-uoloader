using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UOLoader.Server.Entities;

namespace UOLoader.Server.Data
{
    public class LogService
    {
        private readonly UoLoaderContext _context;

        public LogService(UoLoaderContext context) {
            _context = context;
        }

        public async Task<IList<Log>> GetLogsAsync() {
            return await _context.Logs.ToListAsync();
        }
    }
}
