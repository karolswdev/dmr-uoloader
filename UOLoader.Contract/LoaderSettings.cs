using System;

namespace UOLoader.Contract
{
    public class LoaderSettings
    {
        public string ShardName { get; set; }
        public string ShardDescription { get; set; }
        public string ShardUrl { get; set; }
        public string ShardUpdateEndpointUri { get; set; }
        public string LocalUltimaPath { get; set; }
    }
}
