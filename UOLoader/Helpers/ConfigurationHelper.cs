using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UOLoader.Contract;

namespace UOLoader.Helpers
{
    public static class ConfigurationHelper
    {
        public static LoaderSettings GetDefaultSettings() {
            return new LoaderSettings() {
                ShardDescription = "Serwer Ultima Online z zaawansowanymi systemami, z doswiadczona ekipa!",
                ShardName = "Dream Masters Revolution",
                ShardUpdateEndpointUri = "http://64.69.36.187:5000/api/status",
                ShardUrl = "http://www.ultima-dm.pl",
                LocalUltimaPath = Constants.UoLoaderUltimaOnlinePath
            };
        }

        public static bool Save(this LoaderSettings settings, string path = Constants.UoLoaderDefaultConfigName) {

            var settingsString = JsonConvert.SerializeObject(settings);
            try {
                File.WriteAllText(path, settingsString);
                return true;
            }
            catch (Exception ex) {
                Environment.FailFast("Unable to save configuration settings.", ex);
                return false;
            }
        }
    }
}
