using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace UOLoader.Server.Helpers
{
    public static class UrlHelper
    {
        public static string GetFullUrl(string name, int revision) {
            var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            return $"{config["BASE_URL"]}/api/download/{name}/{revision}";
        }

        public static 
    }
}
