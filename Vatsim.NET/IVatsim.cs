using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    public interface IVatsim
    {
        Task<string> GetMETAR(string icaoCode);
        IReadOnlyList<string> GetMessages();
        IVatsimData Data { get; }
    }
}
