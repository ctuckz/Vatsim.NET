using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Vatsim.NET
{
    /// <summary>
    /// Provides access to data from Vatsim.
    /// </summary>
    public interface IVatsim
    {
        /// <summary>
        /// Gets the current METAR for an airport.
        /// </summary>
        /// <param name="icaoCode">The ICAO code of the airport.</param>
        /// <returns>The METAR for the specified airport, as a string.</returns>
        Task<string> GetMETAR(string icaoCode);

        /// <summary>
        /// Gets messages that should be displayed upon login, if any.
        /// </summary>
        /// <returns>The list of messages.</returns>
        IReadOnlyList<string> GetMessages();

        /// <summary>
        /// Gets data about the Vatsim network.
        /// </summary>
        IVatsimData Data { get; }
    }
}
