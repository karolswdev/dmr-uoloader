using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOLoader.Helpers
{
    public static class ConsoleHelper
    {
        public static void WriteLine(string text) {
            var logText = $"[{DateTime.Now}]: {text}";
            LogHelper.Log(logText);
            Console.WriteLine(logText);
        }
    }
}
