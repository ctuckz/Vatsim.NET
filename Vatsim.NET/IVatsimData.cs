using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    /// <summary>
    /// Data about the Vatsim network.
    /// </summary>
    public interface IVatsimData
    {
        /// <summary>
        /// Gets general data, not specific to any user or flight.
        /// </summary>
        /// <returns>General data.</returns>
        Task<GeneralData> GetGeneralData();

        /// <summary>
        /// Gets data for all of the connected clients, including flightplan and aircraft position data.
        /// </summary>
        /// <returns>The list of data for all connected clients.</returns>
        Task<IReadOnlyList<ClientData>> GetClientData();
    }
}
