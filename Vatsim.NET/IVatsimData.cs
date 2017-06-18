using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    public interface IVatsimData
    {
        Task<GeneralData> GetGeneralData();
        Task<IReadOnlyList<ClientData>> GetClientData();
    }
}
