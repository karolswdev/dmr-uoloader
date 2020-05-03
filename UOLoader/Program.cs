using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Figgle;
using Newtonsoft.Json;
using ShellProgressBar;
using UOLoader.Communication;
using UOLoader.Contract;
using UOLoader.Entities;
using UOLoader.Helpers;

namespace UOLoader
{
    public static class Program {

        private static LoaderSettings _settings;

        static async Task Main(string[] args)
        {
            Console.WriteLine(FiggleFonts.Ogre.Render($"UO Loader {AppInfo.Version}"));
            Console.WriteLine(FiggleFonts.Ogre.Render("by karolswdev"));
            LoadConfig();
            ConsoleHelper.WriteLine($"UO Loader version {AppInfo.Version} - {DateTime.Now}");
            ConsoleHelper.WriteLine($"Proudly serving {_settings.ShardName} - {_settings.ShardDescription}");
            ConsoleHelper.WriteLine("");

            IUpdater updater = new Updater(new HttpClient(), _settings);

            var options = new ProgressBarOptions() {
                ProgressCharacter = '#',
                ProgressBarOnBottom = true,
                BackgroundColor = ConsoleColor.DarkGray,
                ForegroundColor = ConsoleColor.Green,
                ForegroundColorDone = ConsoleColor.Red
            };

            UpdatePayload payload;
            var fileCache = FileCache.Load();

            using (var pbar = new ProgressBar(3, "Kontaktowanie serwera w celu pobrania informacji", options)) {

                payload = await updater.GetPayloadInformation(pbar);
                if (payload == null) {
                    pbar.Tick("Blad w pobieraniu informacji. Sprawdz uoloader.settings. Wiecej informacji w logu.");
                    Console.ReadKey();
                    Environment.Exit(1);
                }

                pbar.ForegroundColor = ConsoleColor.Green;
                pbar.Message = "Pobrano informacje.";
            }

            Console.WriteLine(FiggleFonts.Ogre.Render(payload.MessageOfTheDay));

            if (!fileCache.UoSaved) {

                using (var pbar = new ProgressBar(100, "Pobieranie Ultima Online", options)) {
                    var result = await updater.DownloadUo(payload, pbar);
                    if (!result) {
                        pbar.Tick("Blad w pobieraniu UO");
                        Console.ReadKey();
                        Environment.Exit(1);
                    }
                }

                using (var pbar = new ProgressBar(2, "Rozpakowywanie Ultima Online", options))
                {
                    var unzipResult = await updater.UnzipUo(Path.Combine(_settings.LocalUltimaPath, "uo.zip"), pbar);
                    if (!unzipResult)
                    {
                        pbar.Tick("Blad w pobieraniu UO");
                        Console.ReadKey();
                        Environment.Exit(1);
                    }
                    else {
                        fileCache.UoSaved = true;
                        fileCache.Save();
                    }
                }
            }

            if(payload.UpdateFiles.Any()
            {
                ConsoleHelper.WriteLine("Brak plikow zmian. Gotowe");
            }

            foreach (var file in payload.UpdateFiles) {
                if (!fileCache.HasFile(file)) {
                    updater.DownloadUo()
                }
            }
                Console.ReadKey();
        }

        static void LoadConfig() {
            if (!File.Exists(Constants.UoLoaderDefaultConfigName)) {

                var settings = ConfigurationHelper.GetDefaultSettings();
                settings.Save();
                _settings = settings;
                return;
            }

            var fileContents = File.ReadAllText(Constants.UoLoaderDefaultConfigName);
            var settingsObject = JsonConvert.DeserializeObject<LoaderSettings>(fileContents);

            if (settingsObject == null) {
                settingsObject = ConfigurationHelper.GetDefaultSettings();
                settingsObject.Save();
                _settings = settingsObject;
                return;
            }

            _settings = settingsObject;
        }
    }
}
