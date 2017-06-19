using System;
using System.Collections.Generic;
using System.Text;

namespace Vatsim.NET
{
    /// <summary>
    /// Data regarding a client, such as pilot information and aircraft position.
    /// </summary>
    public class ClientData
    {
        internal ClientData(ClientDataBuilder builder)
        {
            Pilot = builder.Pilot;
            Position = builder.Position;
            FlightPlan = builder.FlightPlan;
        }

        /// <summary>
        /// Gets pilot information.
        /// </summary>
        public Pilot Pilot { get; }

        /// <summary>
        /// Gets information about the current aircraft position and transponder code.
        /// </summary>
        public Position Position { get; }

        /// <summary>
        /// Gets information about the flightplan.
        /// </summary>
        public FlightPlan FlightPlan { get; }
    }

    /// <summary>
    /// Represents a connected pilot.
    /// </summary>
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

        /// <summary>
        /// Gets the pilot's callsign.
        /// </summary>
        public string Callsign { get; }

        /// <summary>
        /// Gets the client ID of the pilot.
        /// </summary>
        public int ClientID { get; }

        /// <summary>
        /// Gets the real name of the pilot.
        /// </summary>
        public string RealName { get; }

        /// <summary>
        /// Gets the current client type.
        /// </summary>
        public string ClientType { get; }

        /// <summary>
        /// Gets the pilot's currently tuned frequency, or null if unknown.
        /// </summary>
        public decimal? Frequency { get; }

        /// <summary>
        /// Gets the server which the pilot is connected to.
        /// </summary>
        public string Server { get; }

        /// <summary>
        /// Gets the pilot's rating.
        /// </summary>
        public string Rating { get; }

        /// <summary>
        /// Gets the time which the pilot last recieved an ATIS, or null if the pilot has not yet recieved an ATIS.
        /// </summary>
        public DateTime? LastAtisRecieved { get; }

        /// <summary>
        /// Gets the time which the pilot logged on.
        /// </summary>
        public DateTime? LogOnTime { get; }
    }

    /// <summary>
    /// The current aircraft position.
    /// </summary>
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

        /// <summary>
        /// Gets the aircraft's latitude.
        /// </summary>
        public decimal Latitude { get; }

        /// <summary>
        /// Gets the aircraft's longitude.
        /// </summary>
        public decimal Longitude { get; }

        /// <summary>
        /// Gets the aircraft's altitude, in feet.
        /// </summary>
        public int Altitude { get; }

        /// <summary>
        /// Gets the aircraft's ground speed, in knots.
        /// </summary>
        public int GroundSpeed { get; }

        /// <summary>
        /// Gets the aircraft's current transponder code.
        /// </summary>
        /// <remarks>All transponder codes are 4 digits. Leading 0's are not represented in the transponder code.</remarks>
        public int Transponder { get; }

        /// <summary>
        /// Gets the aircraft's current heading, in degrees.
        /// </summary>
        public int Heading { get; }

        /// <summary>
        /// Gets the aircraft's current altimeter setting, in imperial units (inches mercury).
        /// </summary>
        public decimal AltimeterInHg { get; }

        /// <summary>
        /// Gets the aircraft's current altimeter setting, in metric units (millibars).
        /// </summary>
        public int AltimeterMb { get; }
    }

    /// <summary>
    /// A filed flightplan.
    /// </summary>
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

        /// <summary>
        /// Gets the aircraft.
        /// </summary>
        public string Aircraft { get; }

        /// <summary>
        /// Gets the planned cruise speed.
        /// </summary>
        public int CruiseSpeed { get; }

        /// <summary>
        /// Gets the planned departure airport.
        /// </summary>
        public string DepartureAirport { get; }

        /// <summary>
        /// Gets the planned altitude. May be in flight level format (i.e. FL360).
        /// </summary>
        public string Altitude { get; }

        /// <summary>
        /// Gets the planned arrival airport.
        /// </summary>
        public string ArrivalAirport { get; }

        /// <summary>
        /// Gets the flight type.
        /// </summary>
        public string FlightType { get; }

        /// <summary>
        /// Gets the planned depature time, in UTC. The time does not include the date.
        /// </summary>
        public string DepartureTime { get; }

        /// <summary>
        /// Gets the estimated length of the flight.
        /// </summary>
        public TimeSpan? EnRouteTime { get; }

        /// <summary>
        /// Gets the planned fuel load, measured as endurance.
        /// </summary>
        public TimeSpan? FuelEndurance { get; }

        /// <summary>
        /// Gets the planned alternative airport, if any.
        /// </summary>
        public string AlternativeAirport { get; }

        /// <summary>
        /// Gets the remarks associated with the flight plan.
        /// </summary>
        public string Remarks { get; }

        /// <summary>
        /// Gets the planned route.
        /// </summary>
        public string Route { get; }
    }
}
