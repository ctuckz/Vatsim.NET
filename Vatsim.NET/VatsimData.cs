using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    internal class VatsimData : IVatsimData
    {
        private DateTime _lastRefresh = DateTime.MinValue;
        private GeneralData _general;
        private IReadOnlyList<ClientData> _clients;

        private const string GeneralSectionHeader = SectionHeaderCharacter + "GENERAL:";
        private const string ClientsSectionHeader = SectionHeaderCharacter + "CLIENTS:";
        private const string DateTimeFormat = "yyyyMMddHHmmss";
        private const string CommentCharacter = ";";
        private const string SectionHeaderCharacter = "!";

        public VatsimData(IVatsimDataLoader loader, IVatsimStatus status)
        {
            Loader = loader ?? throw new ArgumentNullException(nameof(loader));
            Status = status ?? throw new ArgumentNullException(nameof(status));
        }

        private IVatsimDataLoader Loader { get; }
        private IVatsimStatus Status { get; }

        public async Task<GeneralData> GetGeneralData()
        {
            await refreshIfDataExpired();
            return _general;
        }

        public async Task<IReadOnlyList<ClientData>> GetClientData()
        {
            await refreshIfDataExpired();
            return _clients;
        }

        private async Task refreshIfDataExpired()
        {
            if (_lastRefresh < (_general?.ReloadUtc ?? DateTime.MinValue))
            {
                return;
            }

            string[] dataLines = (await Loader.LoadData(Status.GetDataUrl().ToString()))
                                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                .Where(s => !s.StartsWith(CommentCharacter)).ToArray();

            _general = loadGeneralData(dataLines);
            _clients = loadClientData(dataLines);

            _lastRefresh = DateTime.UtcNow;
        }

        private static GeneralData loadGeneralData(string[] dataLines)
        {
            int pos;
            for (pos = 0; pos < dataLines.Length; pos++)
            {
                if (string.Equals(dataLines[pos].Trim(), GeneralSectionHeader, StringComparison.Ordinal))
                {
                    break;
                }
            }

            if(pos >= dataLines.Length)
            {
                throw new Exception("Could not find General section in data feed.");
            }

            // We could use the property name as keys, but there's no heirarchy to the data, so we have no idea where the line is coming from. Just because a line starts with
            // 'VERSION =' doesn't mean it's the version field of the general section. IMO it's safer to assume that the properties are always in the same order
            // and just access them by line index. This is why JSON is a thing.
            string version = getGeneralLineValue(dataLines, ++pos);
            string reload = getGeneralLineValue(dataLines, ++pos);
            string update = getGeneralLineValue(dataLines, ++pos);
            string atisRefresh = getGeneralLineValue(dataLines, ++pos);
            string numConnectedClients = getGeneralLineValue(dataLines, ++pos);

            DateTime reloadUtc = !string.IsNullOrWhiteSpace(reload) ? DateTime.UtcNow.AddMinutes(Convert.ToDouble(reload)) : DateTime.MinValue;
            DateTime updateUtc = !string.IsNullOrWhiteSpace(update) ? DateTime.ParseExact(update, DateTimeFormat, CultureInfo.CurrentCulture.DateTimeFormat) : DateTime.MinValue;

            GeneralData general = new GeneralData(version, reloadUtc, updateUtc, 
                !string.IsNullOrWhiteSpace(atisRefresh) ? Convert.ToInt32(atisRefresh) : 0, 
                !string.IsNullOrWhiteSpace(numConnectedClients) ? Convert.ToInt32(numConnectedClients) : 0);

            return general;
        }

        private static string getGeneralLineValue(string[] lines, int pos)
        {
            if(pos >= lines.Length)
            {
                return string.Empty;
            }

            string line = lines[pos];
            // Line format is: PropertyName = Value
            if (string.IsNullOrWhiteSpace(line))
            {
                return string.Empty;
            }

            int equalsPos = line.IndexOf("=");
            if (equalsPos == -1 || equalsPos + 1 >= line.Length)
            {
                return string.Empty;
            }
            return line.Substring(equalsPos + 1).Trim();
        }

        private static IReadOnlyList<ClientData> loadClientData(string[] dataLines)
        {
            int pos;
            for(pos = 0; pos < dataLines.Length; pos++)
            {
                if (string.Equals(dataLines[pos].Trim(), ClientsSectionHeader, StringComparison.Ordinal))
                {
                    // Skip until the client section
                    break;
                }
            }

            if(pos == 0 || pos >= dataLines.Length - 1)
            {
                // We couldn't find a clients section, or it's empty
                return new List<ClientData>().AsReadOnly();
            }

            List<ClientData> clients = new List<ClientData>();
            for(int i = pos + 1; i < dataLines.Length; i++)
            {
                string line = dataLines[i];
                if(line.StartsWith(SectionHeaderCharacter, StringComparison.Ordinal))
                {
                    // We've reached the end of the clients section
                    break;
                }

                clients.Add(new ClientDataBuilder().BuildFromDataLine(line));
            }

            return clients.AsReadOnly();
        }
    }
}
