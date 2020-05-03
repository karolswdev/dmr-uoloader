using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UOLoader.Server.Entities
{
    public class UpdateFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Revision { get; set; }
        public string Uri { get; set; }
        public string RelativeUri { get; set; }
        public string TargetName { get; set; }
        public long SizeInKb { get; set; }
        public string AssociatedMessage { get; set; }
        public bool RequiresUnzip { get; set; }
    }
}
