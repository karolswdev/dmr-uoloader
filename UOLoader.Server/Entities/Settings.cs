using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UOLoader.Server.Entities
{
    public class Settings
    {
        public Guid Id { get; set; }
        public string MessageOfTheDay { get; set; }
        public string RelativeUoUri { get; set; }
        public long UoSizeInKb { get; set; }
    }
}
