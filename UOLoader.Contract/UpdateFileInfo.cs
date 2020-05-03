using System;
using System.Collections.Generic;
using System.Text;

namespace UOLoader.Contract
{
    public class UpdateFileInfo
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public int Revision { get; set; }
        public bool RequiresUnzip { get; set; }
        public string AssociatedMessage { get; set; }
        public long SizeInKb { get; set; }
    }
}
