using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UOLoader.Server.Entities
{
    public class Log
    {
        public Guid Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime When { get; set; }
        public string HostName { get; set; }
        public string LogText { get; set; }
    }
}
