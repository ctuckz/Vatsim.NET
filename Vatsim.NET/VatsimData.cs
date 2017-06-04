using System;
using System.Collections.Generic;
using System.Text;

namespace Vatsim.NET
{
    internal class VatsimData : IVatsimData
    {
        public VatsimData(IVatsimDataLoader loader)
        {

        }

        private string Data { get; }
    }
}
