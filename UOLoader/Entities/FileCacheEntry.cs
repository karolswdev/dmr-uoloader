using System;
using System.Collections.Generic;
using System.Text;

namespace UOLoader.Entities
{
    [Serializable]
    public class FileCacheEntry
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public int Revision { get; set; }
    }
}
