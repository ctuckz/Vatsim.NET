using System;
using System.Collections.Generic;
using System.Text;

namespace Vatsim.NET
{
    internal class ClientDataBuilder : Builder<ClientData>
    {
        public Pilot Pilot { get; set; }
        public Position Position { get; set; }
        public FlightPlan FlightPlan{ get; set; }

        public override ClientData Build()
        {
            return new ClientData(this);
        }
    }

    internal class PilotBuilder : Builder<Pilot>
    {
        public string Callsign { get; set; }
        public int ClientID { get; set; }
        public string RealName { get; set; }
        public string ClientType { get; set; }
        public decimal? Frequency { get; set; }
        public string Server { get; set; }
        public string Rating { get; set; }
        public DateTime? LastAtisRecieved { get; set; }
        public DateTime? LogOnTime { get; set; }

        public override Pilot Build()
        {
            return new Pilot(this);
        }
    }

    internal class PositionBuilder : Builder<Position>
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Altitude { get; set; }
        public int GroundSpeed { get; set; }
        public int Transponder { get; set; }
        public int Heading { get; set; }
        public decimal AltimeterInHg { get; set; }
        public int AltimeterMb { get; set; }

        public override Position Build()
        {
            return new Position(this);
        }
    }

    internal class FlightPlanBuilder : Builder<FlightPlan>
    {
        public string Aircraft { get; set; }
        public int CruiseSpeed { get; set; }
        public string DepartureAirport { get; set; }
        public int Altitude { get; set; }
        public string ArrivalAirport { get; set; }
        public string FlightType { get; set; }
        public DateTime? DepartureTime { get; set; }
        public TimeSpan? EnRouteTime { get; set; }
        public TimeSpan? FuelEndurance { get; set; }
        public string AlternativeAirport { get; set; }
        public string Remarks { get; set; }
        public string Route { get; set; }

        public override FlightPlan Build()
        {
            return new FlightPlan(this);
        }
    }
}
