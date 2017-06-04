using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    /// <summary>
    /// Provides locations where data can be retrieved from, as well as any messages that should be displayed in your application.
    /// </summary>
    /// <remarks>
    /// See http://status.vatsim.net/status.txt for the data format.
    /// </remarks>
    internal class VatsimStatus : IVatsimStatus
    {
        private readonly Random rand = new Random();

        private bool initialized = false;
        private List<Uri> dataUrls = new List<Uri>();
        private List<Uri> serverUrls = new List<Uri>();
        private List<Uri> metarUrls = new List<Uri>();
        private List<Uri> atisUrls = new List<Uri>();
        private List<Uri> userUrls = new List<Uri>();
        private List<string> messages = new List<string>();

        private string StatusUri = "http://status.vatsim.net/status.txt";

        private const string MessageHeader = "msg0";
        private const string DataUrlHeader = "url0";
        private const string ServerUrlHeader = "url1";
        private const string MoveToHeader = "moveto0";
        private const string MetarHeader = "metar0";
        private const string AtisHeader = "atis0";
        private const string UserHeader = "user0";
        private const string CommentCharacter = ";";

        public VatsimStatus(IVatsimStatusLoader loader)
        {
            Loader = loader;
        }

        private IVatsimStatusLoader Loader { get; }

        public Uri GetDataUrl()
        {
            return getRandomItem(GetAllDataUrls());
        }

        public Uri GetServerUrl()
        {
            return getRandomItem(GetAllServerUrls());
        }

        public Uri GetMetarUrl()
        {
            return getRandomItem(GetAllMetarUrls());
        }

        public Uri GetAtisUrl()
        {
            return getRandomItem(GetAllAtisUrls());
        }

        public Uri GetUserUrl()
        {
            return getRandomItem(GetAllUserUrls());
        }

        public IReadOnlyList<Uri> GetAllDataUrls()
        {
            verifyInitialized();
            return dataUrls.AsReadOnly();
        }

        public IReadOnlyList<Uri> GetAllServerUrls()
        {
            verifyInitialized();
            return serverUrls.AsReadOnly();
        }

        public IReadOnlyList<Uri> GetAllMetarUrls()
        {
            verifyInitialized();
            return metarUrls.AsReadOnly();
        }

        public IReadOnlyList<Uri> GetAllAtisUrls()
        {
            verifyInitialized();
            return atisUrls.AsReadOnly();
        }

        public IReadOnlyList<Uri> GetAllUserUrls()
        {
            verifyInitialized();
            return userUrls.AsReadOnly();
        }

        public IReadOnlyList<string> GetMessages()
        {
            verifyInitialized();
            return messages.AsReadOnly();
        }


        private T getRandomItem<T>(IReadOnlyList<T> items)
        {
            if(items == null || items.Count== 0)
            {
                return default(T);
            }
            return items[rand.Next(items.Count)];
        }

        public async Task Initialize()
        {
            // Per vatsim, only load this data once per application instance
            if (initialized)
            {
                return;
            }

            string status = await Loader.LoadData(StatusUri);
            IEnumerable<string> dataLines = status.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                                  .Where(l => !l.StartsWith(CommentCharacter));

            var redirectInfo = checkRedirect(dataLines);
            if (redirectInfo.Redirect)
            {
                // Update the status url and initialize again
                StatusUri = redirectInfo.NewUrl;
                await Initialize();
                return;
            }

            // No redirect, let's load the data
            foreach(string line in dataLines)
            {
                string value = getLineValue(line);
                if (line.StartsWith(DataUrlHeader))
                {
                    dataUrls.Add(new Uri(value));
                }
                else if (line.StartsWith(ServerUrlHeader))
                {
                    serverUrls.Add(new Uri(value));
                }
                else if (line.StartsWith(MetarHeader))
                {
                    metarUrls.Add(new Uri(value));
                }
                else if (line.StartsWith(AtisHeader))
                {
                    atisUrls.Add(new Uri(value));
                }
                else if (line.StartsWith(UserHeader))
                {
                    userUrls.Add(new Uri(value));
                }
                else if (line.StartsWith(MessageHeader))
                {
                    messages.Add(value);
                }
            }       

            initialized = true;
        }

        private static RedirectInfo checkRedirect(IEnumerable<string> data)
        {
            string redirectLine = data.FirstOrDefault(s => s.StartsWith(MoveToHeader));
            bool redirect = redirectLine != null;
            string newUrl = redirect ? getLineValue(redirectLine) : null;

            return new RedirectInfo(redirect, newUrl);
        }

        private static string getLineValue(string line)
        {
            int equalsPos = line.IndexOf("=");
            if (equalsPos == -1 || equalsPos + 1 >= line.Length)
            {
                return string.Empty;
            }
            return line.Substring(equalsPos + 1).Trim();
        }

        private void verifyInitialized()
        {
            if (!initialized)
            {
                throw new Exception("VatsimStatus not initialized.");
            }
        }

        private struct RedirectInfo
        {
            public RedirectInfo(bool redirect, string newUrl)
            {
                Redirect = redirect;
                NewUrl = newUrl;
            }

            public bool Redirect;
            public string NewUrl;
        }
    }
}
