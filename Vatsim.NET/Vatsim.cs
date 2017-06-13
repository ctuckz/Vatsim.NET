using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    internal class Vatsim : IVatsim
    {
        public Vatsim(IVatsimStatus status, IVatsimData data, IMetarDataLoader metarLoader)
        {
            Status = status;
            Data = data;
            MetarLoader = metarLoader;
        }

        private IVatsimStatus Status { get; }
        private IMetarDataLoader MetarLoader { get; }

        public async Task<string> GetMETAR(string icaoCode)
        {
            if (string.IsNullOrWhiteSpace(icaoCode))
            {
                throw new ArgumentNullException(nameof(icaoCode));
            }

            Uri metarUrl = Status.GetMetarUrl();
            if(metarUrl == null)
            {
                throw new Exception("METAR functionality is currently unavailable.");
            }

            string metar = await MetarLoader.LoadData(metarUrl.ToString(), icaoCode);
            if(string.IsNullOrEmpty(metar) || metar.StartsWith("No METAR available for"))
            {
                throw new Exception($"METAR is unavailable for {icaoCode}");
            }

            return metar;
        }

        public IReadOnlyList<string> GetMessages()
        {
            return Status.GetMessages();
        }

        public IVatsimData Data { get; }
    }
}
