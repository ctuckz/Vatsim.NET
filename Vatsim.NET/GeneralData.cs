using System;
using System.Collections.Generic;
using System.Text;

namespace Vatsim.NET
{
    /// <summary>
    /// General Vatsim data, not associated with any specific client.
    /// </summary>
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

        /// <summary>
        /// The number of pilots currently connected to Vatsim.
        /// </summary>
        public int NumConnectedClients { get; }

        internal string Version { get; }
        internal DateTime ReloadUtc { get; }
        internal DateTime LastUpdateUtc { get; }
        internal int AtisRefreshInterval { get; }
    }
}
