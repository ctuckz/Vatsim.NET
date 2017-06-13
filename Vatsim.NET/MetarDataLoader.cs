using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    internal class MetarDataLoader : IMetarDataLoader
    {
        public async Task<string> LoadData(string dataUrl, string icaoCode)
        {
            if (string.IsNullOrWhiteSpace(dataUrl))
            {
                throw new ArgumentNullException(nameof(dataUrl));
            }

            Uri dataUri = new Uri(dataUrl);
            if (string.IsNullOrWhiteSpace(dataUri.Query))
            {
                dataUrl += "?id={0}";
            }

            dataUrl = string.Format(dataUrl, icaoCode);

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(dataUri))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Could not get metar for {icaoCode}. Response code: {response.StatusCode}");
                }

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
