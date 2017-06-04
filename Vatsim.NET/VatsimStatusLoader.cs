using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    internal class VatsimStatusLoader : IVatsimStatusLoader
    {
        public async Task<string> LoadData(string statusUri)
        {
            if (string.IsNullOrWhiteSpace(statusUri))
            {
                throw new ArgumentNullException(nameof(statusUri));
            }

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(statusUri))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        // TODO: Custom exception types
                        throw new HttpRequestException($"Could not load status page at {statusUri}. Response code: {response.StatusCode}");
                    }
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
