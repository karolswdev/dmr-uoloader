using System;
using System.Collections.Generic;
using System.Text;

namespace UOLoader.Contract
{
    public class UpdatePayload
    {
        public string MessageOfTheDay { get; set; }
        public string FullUltimaOnlineDownloadPath { get; set; }
        public long FullUltimaOnlineDownloadWeightInKb { get; set; }

        public IList<UpdateFileInfo> UpdateFiles { get; set; }
    }
}
