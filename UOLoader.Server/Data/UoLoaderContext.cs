using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UOLoader.Server.Entities;

namespace UOLoader.Server.Data
{
    public class UoLoaderContext : DbContext
    {
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<UpdateFile> UpdateFiles { get; set; }

        public virtual DbSet<Log> Logs { get; set; }

        public UoLoaderContext(DbContextOptions<UoLoaderContext> options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlite($"Data Source={Constants.DataDirectory}uoloader.db");
            }
            base.OnConfiguring(optionsBuilder);
        }
    }
}
