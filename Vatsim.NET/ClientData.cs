using System;
using System.Collections.Generic;
using System.Text;

namespace Vatsim.NET
{
    public class ClientData
    {
        internal ClientData() { }

        public Pilot Pilot { get; }
        public Position Position { get; }
        public FlightPlan FlightPlan { get; }
    }

    public class Pilot
    {
        internal Pilot() { }

        public string Callsign { get; }
        public int ClientID { get; }
        public string RealName { get; }
        public string ClientType { get; }
        public decimal? Frequency { get; }
        public string Server { get; }
        public string Rating { get; }
        public DateTime? LastAtisRecieved { get; }
        public DateTime? LogOnTime { get; }
    }

    public class Position
    {
        public decimal Latitude { get; }
        public decimal Longitude { get; }
        public int Altitude { get; }
        public int GroundSpeed { get; }
        public int Transponder { get; }
        public int Heading { get; }
        public decimal AltimeterInHg { get; }
        public int AltimeterMb { get; }
    }

    public class FlightPlan
    {
        internal FlightPlan() { }

        public string Aircraft { get; }
        public int CruiseSpeed { get; }
        public string DepartureAirprot { get; }
        public int Altitude { get; }
        public string ArrivalAirport { get; }
        public string FlightType { get; }
        public DateTime? DepartureTime { get; }
        public TimeSpan? EnRouteTime { get; }
        public TimeSpan? FuelEndurance { get; }
        public string AlternativeAirport { get; }
        public string Remarks { get; }
        public string Route { get; }
    }
}
