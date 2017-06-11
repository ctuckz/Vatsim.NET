using System;
using System.Collections.Generic;
using System.Text;

namespace Vatsim.NET
{
    public class ClientData
    {
        internal ClientData(ClientDataBuilder builder)
        {
            Pilot = builder.Pilot;
            Position = builder.Position;
            FlightPlan = builder.FlightPlan;
        }

        public Pilot Pilot { get; }
        public Position Position { get; }
        public FlightPlan FlightPlan { get; }
    }

    public class Pilot
    {
        internal Pilot(PilotBuilder builder)
        {
            Callsign = builder.Callsign;
            ClientID = builder.ClientID;
            RealName = builder.RealName;
            ClientType = builder.ClientType;
            Frequency = builder.Frequency;
            Server = builder.Server;
            Rating = builder.Rating;
            LastAtisRecieved = builder.LastAtisRecieved;
            LogOnTime = builder.LogOnTime;
        }

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
        internal Position(PositionBuilder builder)
        {
            Latitude = builder.Latitude;
            Longitude = builder.Longitude;
            Altitude = builder.Altitude;
            GroundSpeed = builder.GroundSpeed;
            Transponder = builder.Transponder;
            Heading = builder.Heading;
            AltimeterInHg = builder.AltimeterInHg;
            AltimeterMb = builder.AltimeterMb;
        }

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
        internal FlightPlan(FlightPlanBuilder builder)
        {
            Aircraft = builder.Aircraft;
            CruiseSpeed = builder.CruiseSpeed;
            DepartureAirport = builder.DepartureAirport;
            Altitude = builder.Altitude;
            ArrivalAirport = builder.ArrivalAirport;
            FlightType = builder.FlightType;
            DepartureTime = builder.DepartureTime;
            EnRouteTime = builder.EnRouteTime;
            FuelEndurance = builder.FuelEndurance;
            AlternativeAirport = builder.AlternativeAirport;
            Remarks = builder.Remarks;
            Route = builder.Route;
        }

        public string Aircraft { get; }
        public int CruiseSpeed { get; }
        public string DepartureAirport { get; }
        /// <summary>
        /// Gets the planned altitude. May be in flight level format (i.e. FL360).
        /// </summary>
        public string Altitude { get; }
        public string ArrivalAirport { get; }
        public string FlightType { get; }
        /// <summary>
        /// Gets the planned depature time, in UTC.
        /// </summary>
        public string DepartureTime { get; }
        public TimeSpan? EnRouteTime { get; }
        public TimeSpan? FuelEndurance { get; }
        public string AlternativeAirport { get; }
        public string Remarks { get; }
        public string Route { get; }
    }
}
