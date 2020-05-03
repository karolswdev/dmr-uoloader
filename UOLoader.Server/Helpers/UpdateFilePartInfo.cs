using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UOLoader.Server.Helpers
{
    public class UpdateFilePartInfo
    {
        public int Revision { get; set; }
        public string Name { get; set; }
        public string RelativeUri { get; set; }
        public long SizeInKb { get; set; }
        public bool RequiresUnzip { get; set; }

        // Update files are comprised physically the following way:
        // e.g. map1__V1__.zip
        // name: map1
        // revision: 1
        // requiresUnzip: true
        // another example:
        // tiledata__V3__.mul
        // name: tiledata
        // revision: 3
        // requiresUnzip: false
        public static UpdateFilePartInfo GetFromPath(string path) {

            if (!File.Exists(path)) {
                return null;
            }

            var fi = new FileInfo(path);

            if (!fi.Name.Contains("__V")) {
                return null;
            }

            var nameIndex = fi.Name.IndexOf("__V", StringComparison.InvariantCulture);

            if (nameIndex <= 0) {
                return null;
            }

            var name = fi.Name.Substring(0, nameIndex);

            if (String.IsNullOrEmpty(name)) {
                return null;
            }

            var revisionIndex = fi.Name.IndexOf("__", nameIndex + 3, StringComparison.CurrentCulture);

            var revisionString = fi.Name.Substring(nameIndex + 3, revisionIndex);

            var revisionNumber = 0;

            if (!Int32.TryParse(revisionString, out revisionNumber)) {
                return null;
            }

            return new UpdateFilePartInfo() {
                Name = name,
                RelativeUri = fi.Name,
                RequiresUnzip = fi.Name.ToLower().Contains(".zip"),
                Revision = revisionNumber,
                SizeInKb = fi.Length / 1024
            };


        }

    }

    
}
