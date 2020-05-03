using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ShellProgressBar;
using UOLoader.Contract;

namespace UOLoader.Communication {
    public interface IUpdater {
        Task<UpdatePayload> GetPayloadInformation(ProgressBar pbar);
        Task<bool> UnzipFile(FileInfo file, ProgressBar pBar, string filePath);
        Task<bool> DownloadFile(FileInfo file, ProgressBar pBar, string filePath);
        Task<bool> DownloadUo(UpdatePayload payload, ProgressBar pBar);
        Task<bool> UnzipUo(string filePath, ProgressBar pBar);
    }
}
