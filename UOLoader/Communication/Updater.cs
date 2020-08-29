using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using ShellProgressBar;
using UOLoader.Contract;
using UOLoader.Helpers;

namespace UOLoader.Communication
{
    public class Updater : IUpdater {

        private readonly HttpClient _httpClient;
        private readonly LoaderSettings _settings;

        public Updater(HttpClient client, LoaderSettings settings) {
            _httpClient = client;
            _settings = settings;
        }

        public async Task<UpdatePayload> GetPayloadInformation(ProgressBar pbar) {
            try {
                pbar.Tick("Wysylanie informacji do serwera.");
                var response = await _httpClient.GetAsync(_settings.ShardUpdateEndpointUri);

                if (!response.IsSuccessStatusCode)
                {
                    pbar.Tick("Wyslano informacje, bledny kod powrotu");
                    LogHelper.Log($"Got an unsuccessful status code when attempting to reach {_settings.ShardUpdateEndpointUri}");
                    return null;
                }

                pbar.Tick("Odczytywanie informacji zwrotnej");
                var responseBody = await response.Content.ReadAsStringAsync();
                var updatePayload = JsonConvert.DeserializeObject<UpdatePayload>(responseBody);
                pbar.Tick("Deserializacja informacji zwrotnej.");
                if (updatePayload == null)
                {
                    pbar.Tick("Blad w deserializacji informacji zwrotnej.");
                    LogHelper.Log("Got a corrupt response from endpoint");
                    return null;
                }
                return updatePayload;
            }
            catch (HttpRequestException requestException) {
                LogHelper.Log($"Got a request exception from endpoint {_settings.ShardUpdateEndpointUri} - {requestException.Message}");
                return null;
            }

          
        }

        public async Task<bool> UnzipFile(FileInfo file, ProgressBar pBar, string destFileName) {

            var fullPath = Path.Combine(_settings.LocalUltimaPath, destFileName);

            if (!File.Exists(fullPath))
            {
                return false;
            }

            pBar.Tick($"Rozpakowywanie {destFileName}");
            var targetDirectory = _settings.LocalUltimaPath;
            var zip = new FastZip();
            zip.ExtractZip(fullPath, targetDirectory, null);
            return true;
        }

        public async Task<bool> DownloadFile(UpdateFileInfo file, ProgressBar pBar, string destFileName) {

            
            var fullPath = Path.Combine(_settings.LocalUltimaPath, destFileName);
            var uri = file.Uri;

            using (var response = _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead).Result)
            {
                response.EnsureSuccessStatusCode();

                using (Stream contentStream = await response.Content.ReadAsStreamAsync(), fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var totalRead = 0L;
                    var totalReads = 0L;
                    var buffer = new byte[8192];
                    var isMoreToRead = true;
                    var percentage = 0;

                    do
                    {
                        var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                        if (read == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            await fileStream.WriteAsync(buffer, 0, read);

                            totalRead += read;
                            totalReads += 1;

                            var percentageTotal =
                                (int)Math.Round(
                                    ((decimal)totalRead / (file.SizeInKb * 10)), 0);


                            if (percentageTotal > percentage)
                            {
                                pBar.Tick();
                                percentage = percentageTotal;
                            }

                        }
                    }
                    while (isMoreToRead);
                }
            }
            return true;
        }

        public async Task<bool> DownloadUo(UpdatePayload payload, ProgressBar pbar) {

            var filePath = _settings.LocalUltimaPath;

            if (!Directory.Exists(filePath)) {
                Directory.CreateDirectory(filePath);
            }
            var uri = payload.FullUltimaOnlineDownloadPath;

            using (var response = _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead).Result)
            {
                response.EnsureSuccessStatusCode();

                using (Stream contentStream = await response.Content.ReadAsStreamAsync(), fileStream = new FileStream(Path.Combine(filePath, "uo.zip"), FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var totalRead = 0L;
                    var totalReads = 0L;
                    var total = payload.FullUltimaOnlineDownloadWeightInKb * 100L;
                    var buffer = new byte[8192];
                    var isMoreToRead = true;

                    var percentage = 0;

                    do
                    {
                        var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                        if (read == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            await fileStream.WriteAsync(buffer, 0, read);

                            totalRead += read;
                            totalReads += 1;

                            var percentageTotal =
                                (int) Math.Round(
                                    ((decimal) totalRead / (payload.FullUltimaOnlineDownloadWeightInKb * 10)), 0);


                            if (percentageTotal > percentage) {
                                pbar.Tick();
                                percentage = percentageTotal;
                            }
                        }
                    }
                    while (isMoreToRead);
                }
            }
            return true;
        }

        public async Task<bool> UnzipUo(string filePath, ProgressBar pBar) {

            var fullPath = Path.Combine(_settings.LocalUltimaPath, "uo.zip");

            if (!File.Exists(fullPath)) {
                return false;
            }

            pBar.Tick("Rozpakowywanie UO");
            var targetDirectory = _settings.LocalUltimaPath;
            var zip = new FastZip();
            zip.ExtractZip(fullPath, targetDirectory, null);
            pBar.Tick("UO Rozpakowane");
            return true;
        }
    }
}
