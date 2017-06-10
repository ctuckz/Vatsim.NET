using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    internal class VatsimDataLoader : IVatsimDataLoader
    {
        public async Task<string> LoadData(string dataUrl)
        {
            if (string.IsNullOrWhiteSpace(dataUrl))
            {
                throw new ArgumentNullException(nameof(dataUrl));
            }

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(dataUrl))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Could not load the data page at {dataUrl}. Response code: {response.StatusCode}");
                }

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
