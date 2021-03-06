﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vatsim.NET
{
    internal static class ClientDataBuilderExtensions
    {
        private const char FieldSplitCharacter = ':';
        private static readonly Dictionary<string, int> ClientFieldPositions;

        static ClientDataBuilderExtensions()
        {
            string[] splitFields = "callsign:cid:realname:clienttype:frequency:latitude:longitude:altitude:groundspeed:planned_aircraft:planned_tascruise:planned_depairport:planned_altitude:planned_destairport:server:protrevision:rating:transponder:facilitytype:visualrange:planned_revision:planned_flighttype:planned_deptime:planned_actdeptime:planned_hrsenroute:planned_minenroute:planned_hrsfuel:planned_minfuel:planned_altairport:planned_remarks:planned_route:planned_depairport_lat:planned_depairport_lon:planned_destairport_lat:planned_destairport_lon:atis_message:time_last_atis_received:time_logon:heading:QNH_iHg:QNH_Mb:"
                .Split(FieldSplitCharacter);

            ClientFieldPositions = new Dictionary<string, int>();
            for(int i = 0; i < splitFields.Length; i++)
            {
                ClientFieldPositions.Add(splitFields[i], i);
            }
        }

        public static ClientData BuildFromDataLine(this ClientDataBuilder builder, string dataLine)
        {
            string[] fields = dataLine.Split(FieldSplitCharacter);

            PilotBuilder pilotBuilder = new PilotBuilder
            {
                Callsign = fields[ClientFieldPositions["callsign"]],
                ClientID = fields[ClientFieldPositions["cid"]].ToInt(),
                ClientType = fields[ClientFieldPositions["clienttype"]],
                Frequency = fields[ClientFieldPositions["frequency"]].ToDecimal(),
                LastAtisRecieved = fields[ClientFieldPositions["time_last_atis_received"]].ToDateTime(),
                LogOnTime = fields[ClientFieldPositions["time_logon"]].ToDateTime(),
                Rating = fields[ClientFieldPositions["rating"]],
                RealName = fields[ClientFieldPositions["realname"]],
                Server = fields[ClientFieldPositions["server"]]
            };
            builder.Pilot = pilotBuilder.Build();

            PositionBuilder positionBuilder = new PositionBuilder
            {
                AltimeterInHg = fields[ClientFieldPositions["QNH_iHg"]].ToDecimal(),
                AltimeterMb = fields[ClientFieldPositions["QNH_Mb"]].ToInt(),
                Altitude = fields[ClientFieldPositions["altitude"]].ToInt(),
                GroundSpeed = fields[ClientFieldPositions["groundspeed"]].ToInt(),
                Heading = fields[ClientFieldPositions["heading"]].ToInt(),
                Latitude = fields[ClientFieldPositions["latitude"]].ToDecimal(),
                Longitude = fields[ClientFieldPositions["longitude"]].ToDecimal(),
                Transponder = fields[ClientFieldPositions["transponder"]].ToInt()
            };
            builder.Position = positionBuilder.Build();

            FlightPlanBuilder flightPlanBuilder = new FlightPlanBuilder
            {
                Aircraft = fields[ClientFieldPositions["planned_aircraft"]],
                AlternativeAirport = fields[ClientFieldPositions["planned_altairport"]],
                Altitude = fields[ClientFieldPositions["planned_altitude"]],
                ArrivalAirport = fields[ClientFieldPositions["planned_destairport"]],
                CruiseSpeed = fields[ClientFieldPositions["planned_tascruise"]].ToInt(),
                DepartureAirport = fields[ClientFieldPositions["planned_depairport"]],
                DepartureTime = fields[ClientFieldPositions["planned_deptime"]],
                FlightType = fields[ClientFieldPositions["planned_flighttype"]],
                Remarks = fields[ClientFieldPositions["planned_remarks"]],
                Route = fields[ClientFieldPositions["planned_route"]]
            };
            int enRouteHours = fields[ClientFieldPositions["planned_hrsenroute"]].ToInt();
            int enRouteMins = fields[ClientFieldPositions["planned_minenroute"]].ToInt();
            flightPlanBuilder.EnRouteTime = new TimeSpan(enRouteHours, enRouteMins, 0);

            int enduranceHours = fields[ClientFieldPositions["planned_hrsfuel"]].ToInt();
            int enduranceMins = fields[ClientFieldPositions["planned_minfuel"]].ToInt();
            flightPlanBuilder.FuelEndurance = new TimeSpan(enduranceHours, enduranceMins, 0);

            builder.FlightPlan = flightPlanBuilder.Build();

            return new ClientData(builder);
        }
    }
}
