using KP.OrderMGT.BL.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace KP.OrderMGT.BL.ServiceModel
{

    [DataContract]
    public enum WeekDays
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }

    [DataContract]
    public enum Terminal
    {
        Departure = 1,  // ออก
        Arrival = 2,    // เข้า
        Tranfer = 3     // เปลี่ยน  
    }

    [DataContract]
    public class FlightsAll
    {
        [DataMember]
        public List<Flight> Arrival { get; set; }

        [DataMember]
        public List<Flight> Departure { get; set; }

        [DataMember]
        public List<Flight> Transfer { get; set; }

    }

    [DataContract]
    public class Flight
    {
        [DataMember]
        public string FightCode { get; set; }

        [DataMember]
        public string FightDesc { get; set; }

        [DataMember]
        public string AirportCode { get; set; }

        [DataMember]
        public string AirlineCode { get; set; }

        [DataMember]
        public string AirlineName { get; set; }

        [DataMember]
        public string Teminal { get; set; }

        [DataMember]
        public string DepartureAirport { get; set; }

        [DataMember]
        public string ArrivalAirport { get; set; }

        [DataMember]
        public FlightTime Time { get; set; }

        [DataMember]
        public List<FlightWeekDays> WeekDays { get; set; }

        public Flight() { }

        public Flight(df_flight dataTable1)
        {
            this.FightCode = dataTable1.flight_code.Trim();
            this.FightDesc = dataTable1.flight_desc.Trim();
            this.Teminal = dataTable1.arrdep_terminal.Trim();
            if (this.Teminal == "D")
            {
                this.AirportCode = dataTable1.arrdep_airport_code.Trim();
            }
            else if (this.Teminal == "A")
            {
                this.AirportCode = dataTable1.dest_airport_code.Trim();
            }
            else
            {
                this.AirportCode = dataTable1.arrdep_airport_code.Trim();
            }

            this.DepartureAirport = dataTable1.dest_airport_code.Trim();
            this.ArrivalAirport = dataTable1.arrdep_airport_code.Trim();
            this.Time = new FlightTime();
            this.Time.Time24 = dataTable1.arrdep_time.Trim();
            this.Time.TimeAMPM = dataTable1.arrdep_time.Trim();
            this.WeekDays = new List<FlightWeekDays>();
            if (dataTable1.d1)
            {
                var d1 = new FlightWeekDays();
                d1.ShortName = "MON";
                d1.FullName = ServiceModel.WeekDays.Monday.ToString();
                this.WeekDays.Add(d1);
            }

            if (dataTable1.d2)
            {
                var d2 = new FlightWeekDays();
                d2.ShortName = "TUE";
                d2.FullName = ServiceModel.WeekDays.Tuesday.ToString();
                this.WeekDays.Add(d2);
            }

            if (dataTable1.d3)
            {
                var d3 = new FlightWeekDays();
                d3.ShortName = "WED";
                d3.FullName = ServiceModel.WeekDays.Wednesday.ToString();
                this.WeekDays.Add(d3);
            }

            if (dataTable1.d4)
            {
                var d4 = new FlightWeekDays();
                d4.ShortName = "THU";
                d4.FullName = ServiceModel.WeekDays.Thursday.ToString();
                this.WeekDays.Add(d4);
            }

            if (dataTable1.d5)
            {
                var d5 = new FlightWeekDays();
                d5.ShortName = "FRI";
                d5.FullName = ServiceModel.WeekDays.Friday.ToString();
                this.WeekDays.Add(d5);
            }

            if (dataTable1.d6)
            {
                var d6 = new FlightWeekDays();
                d6.ShortName = "STU";
                d6.FullName = ServiceModel.WeekDays.Saturday.ToString();
                this.WeekDays.Add(d6);
            }

            if (dataTable1.d7)
            {
                var d7 = new FlightWeekDays();
                d7.ShortName = "SUN";
                d7.FullName = ServiceModel.WeekDays.Sunday.ToString();
                this.WeekDays.Add(d7);
            }


        }

        public Flight(df_flight dataTable1, df_airline dataTable2)
        {
            this.FightCode = dataTable1.flight_code.Trim();
            this.FightDesc = dataTable1.flight_desc.Trim();
            this.Teminal = dataTable1.arrdep_terminal.Trim();
            if (this.Teminal == "D")
            {
                this.AirportCode = dataTable1.arrdep_airport_code.Trim();
            }
            else if (this.Teminal == "A")
            {
                this.AirportCode = dataTable1.dest_airport_code.Trim();
            }
            else
            {
                this.AirportCode = dataTable1.arrdep_airport_code.Trim();
            }

            this.AirlineCode = dataTable2.airline_code.Trim();
            this.AirlineName = dataTable2.airline_desc.Trim();
            this.DepartureAirport = dataTable1.dest_airport_code.Trim();
            this.ArrivalAirport = dataTable1.arrdep_airport_code.Trim();
            this.Time = new FlightTime();
            this.Time.Time24 = dataTable1.arrdep_time.Trim();
            this.Time.TimeAMPM = dataTable1.arrdep_time.Trim();
            this.WeekDays = new List<FlightWeekDays>();
            if (dataTable1.d1)
            {
                var d1 = new FlightWeekDays();
                d1.ShortName = "MON";
                d1.FullName = ServiceModel.WeekDays.Monday.ToString();
                this.WeekDays.Add(d1);
            }

            if (dataTable1.d2)
            {
                var d2 = new FlightWeekDays();
                d2.ShortName = "TUE";
                d2.FullName = ServiceModel.WeekDays.Tuesday.ToString();
                this.WeekDays.Add(d2);
            }

            if (dataTable1.d3)
            {
                var d3 = new FlightWeekDays();
                d3.ShortName = "WED";
                d3.FullName = ServiceModel.WeekDays.Wednesday.ToString();
                this.WeekDays.Add(d3);
            }

            if (dataTable1.d4)
            {
                var d4 = new FlightWeekDays();
                d4.ShortName = "THU";
                d4.FullName = ServiceModel.WeekDays.Thursday.ToString();
                this.WeekDays.Add(d4);
            }

            if (dataTable1.d5)
            {
                var d5 = new FlightWeekDays();
                d5.ShortName = "FRI";
                d5.FullName = ServiceModel.WeekDays.Friday.ToString();
                this.WeekDays.Add(d5);
            }

            if (dataTable1.d6)
            {
                var d6 = new FlightWeekDays();
                d6.ShortName = "STU";
                d6.FullName = ServiceModel.WeekDays.Saturday.ToString();
                this.WeekDays.Add(d6);
            }

            if (dataTable1.d7)
            {
                var d7 = new FlightWeekDays();
                d7.ShortName = "SUN";
                d7.FullName = ServiceModel.WeekDays.Sunday.ToString();
                this.WeekDays.Add(d7);
            }


        }

    }

    [DataContract]
    public class FlightCarrier
    {
        [DataMember]
        public string Fs { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FlightNumber { get; set; }
    }

    [DataContract]
    public class FlightTime
    {
        [DataMember]
        public string TimeAMPM { get; set; }

        [DataMember]
        public string Time24 { get; set; }
    }

    [DataContract]
    public class FlightWeekDays
    {
        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public string FullName { get; set; }
    }
}
