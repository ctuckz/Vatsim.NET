using System;
using System.Collections.Generic;
using System.Text;

namespace Vatsim.NET
{
    public class GeneralData
    {
        internal GeneralData(string version, DateTime reloadUTC, DateTime lastUpdateUTC, int atisRefreshInterval, int numConnectedClients)
        {
            Version = version;
            ReloadUtc = reloadUTC;
            LastUpdateUtc = lastUpdateUTC;
            AtisRefreshInterval = atisRefreshInterval;
            NumConnectedClients = numConnectedClients;
        }

        public string Version { get; }
        public DateTime ReloadUtc { get; }
        public DateTime LastUpdateUtc { get; }
        public int AtisRefreshInterval { get; }
        public int NumConnectedClients { get; }
    }
}
