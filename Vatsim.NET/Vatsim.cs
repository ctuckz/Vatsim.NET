using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    internal class Vatsim : IVatsim
    {
        public Vatsim(IVatsimStatus status)
        {
            Status = status;
        }

        private IVatsimStatus Status { get; }

        public string GetMETAR(string icaoCode)
        {
            if (string.IsNullOrWhiteSpace(icaoCode))
            {
                throw new ArgumentNullException(nameof(icaoCode));
            }

            return null;
        }

        public IReadOnlyList<string> GetMessages()
        {
            return Status.GetMessages();
        }
    }
}
