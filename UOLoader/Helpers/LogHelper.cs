using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOLoader.Helpers
{
    public static class LogHelper
    {
        public static void Log(string text) {

            try {
                File.AppendAllText("./loader.log", text);
            }
            // Bad suppression, ad-hoc code.
            catch (Exception ex) {
                return;
            }

        }
    }
}
