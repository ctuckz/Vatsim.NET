using System;
using System.Collections.Generic;
using System.Text;

namespace Vatsim.NET
{
    interface IVatsim
    {
        string GetMETAR(string icaoCode);
        IReadOnlyList<string> GetMessages();
        IVatsimData Data { get; }
    }
}
