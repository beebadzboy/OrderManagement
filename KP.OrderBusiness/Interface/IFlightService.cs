using KP.OrderBusiness.DBModel;
using KP.OrderBusiness.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace KP.OrderBusiness.Interface
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFlightService" in both code and config file together.
    [ServiceContract]
    public interface IFlightService
    {
        [OperationContract]
        Flight CheckFlights(string fight_code);

        [OperationContract]
        Flight CheckFlights(string fight_code, string fight_date);

        [OperationContract]
        Flight GetDataFlights(string fight_code);

        [OperationContract]
        FlightsAll GetDataAll();

        [OperationContract]
        List<Flight> GetDataDeparture();

        [OperationContract]
        List<Flight> GetDataArrival();

        [OperationContract]
        List<Flight> GetDataTransfer();
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
        public string FlightCode { get; set; }

        [DataMember]
        public string FlightDesc { get; set; }

        [DataMember]
        public DateTime? Date { get; set; }

        [DataMember]
        public string AirportCode { get; set; }

        [DataMember]
        public string AirlineCode { get; set; }

        [DataMember]
        public string AirlineName { get; set; }

        [DataMember]
        public string Terminal { get; set; }

        [DataMember]
        public PickUp PickUp { get; set; }

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
            this.FlightCode = dataTable1.flight_code.Trim();
            this.FlightDesc = dataTable1.flight_desc.Trim();
            this.Terminal = dataTable1.arrdep_terminal.Trim();
            if (this.Terminal == "D")
            {
                this.AirportCode = dataTable1.arrdep_airport_code.Trim();
                this.DepartureAirport = dataTable1.arrdep_airport_code.Trim();
                this.ArrivalAirport = dataTable1.dest_airport_code.Trim();
            }
            else if (this.Terminal == "A")
            {
                this.AirportCode = dataTable1.dest_airport_code.Trim();
                this.DepartureAirport = dataTable1.arrdep_airport_code.Trim();
                this.ArrivalAirport = dataTable1.dest_airport_code.Trim();
            }
            else
            {
                string[] airport = new string[] { "BKK", "DMK", "HKT", "UTP" };
                if (airport.Contains(dataTable1.dest_airport_code.Trim()))
                {
                    // remark Z(A)
                    this.AirportCode = dataTable1.dest_airport_code.Trim();
                    this.DepartureAirport = dataTable1.dest_airport_code.Trim();
                    this.ArrivalAirport = dataTable1.arrdep_airport_code.Trim();
                }
                else
                {
                    // remark Z(D)
                    this.AirportCode = dataTable1.arrdep_airport_code.Trim();
                    this.DepartureAirport = dataTable1.arrdep_airport_code.Trim();
                    this.ArrivalAirport = dataTable1.dest_airport_code.Trim();
                }
            }

            this.Time = new FlightTime();
            this.Time.Time24 = dataTable1.arrdep_time.Trim();
            TimeSpan timespan = TimeSpan.Parse(dataTable1.arrdep_time.Trim());
            DateTime time = DateTime.Today.Add(timespan);
            this.Time.TimeAMPM = time.ToString("hh:mm tt"); // It will give "03:00 AM"
            this.WeekDays = new List<FlightWeekDays>();
            if (dataTable1.d2)
            {
                var d2 = new FlightWeekDays();
                d2.ShortName = "MON";
                d2.FullName = ServiceModel.WeekDays.Monday.ToString();
                this.WeekDays.Add(d2);
            }

            if (dataTable1.d3)
            {
                var d3 = new FlightWeekDays();
                d3.ShortName = "TUE";
                d3.FullName = ServiceModel.WeekDays.Tuesday.ToString();
                this.WeekDays.Add(d3);
            }

            if (dataTable1.d4)
            {
                var d4 = new FlightWeekDays();
                d4.ShortName = "WED";
                d4.FullName = ServiceModel.WeekDays.Wednesday.ToString();
                this.WeekDays.Add(d4);
            }

            if (dataTable1.d5)
            {
                var d5 = new FlightWeekDays();
                d5.ShortName = "THU";
                d5.FullName = ServiceModel.WeekDays.Thursday.ToString();
                this.WeekDays.Add(d5);
            }

            if (dataTable1.d6)
            {
                var d6 = new FlightWeekDays();
                d6.ShortName = "FRI";
                d6.FullName = ServiceModel.WeekDays.Friday.ToString();
                this.WeekDays.Add(d6);
            }

            if (dataTable1.d7)
            {
                var d7 = new FlightWeekDays();
                d7.ShortName = "STU";
                d7.FullName = ServiceModel.WeekDays.Saturday.ToString();
                this.WeekDays.Add(d7);
            }

            if (dataTable1.d7)
            {
                var d1 = new FlightWeekDays();
                d1.ShortName = "SUN";
                d1.FullName = ServiceModel.WeekDays.Sunday.ToString();
                this.WeekDays.Add(d1);
            }


        }

        public Flight(df_flight dataTable1, df_airline dataTable2)
        {
            this.FlightCode = dataTable1.flight_code.Trim();
            this.FlightDesc = dataTable1.flight_desc.Trim();
            this.Terminal = dataTable1.arrdep_terminal.Trim();
            if (this.Terminal == "D")
            {
                this.AirportCode = dataTable1.arrdep_airport_code.Trim();
                this.DepartureAirport = dataTable1.arrdep_airport_code.Trim();
                this.ArrivalAirport = dataTable1.dest_airport_code.Trim();
            }
            else if (this.Terminal == "A")
            {
                this.AirportCode = dataTable1.dest_airport_code.Trim();
                this.DepartureAirport = dataTable1.arrdep_airport_code.Trim();
                this.ArrivalAirport = dataTable1.dest_airport_code.Trim();
            }
            else
            {
                string[] airport = new string[] { "BKK", "DMK", "HKT", "UTP" };
                if (airport.Contains(dataTable1.dest_airport_code.Trim()))
                {
                    // remark Z(A)
                    this.AirportCode = dataTable1.dest_airport_code.Trim();
                    this.DepartureAirport = dataTable1.arrdep_airport_code.Trim();
                    this.ArrivalAirport = dataTable1.dest_airport_code.Trim();
                }
                else
                {
                    // remark Z(D)
                    this.AirportCode = dataTable1.arrdep_airport_code.Trim();
                    this.DepartureAirport = dataTable1.arrdep_airport_code.Trim();
                    this.ArrivalAirport = dataTable1.dest_airport_code.Trim();
                }
            }
            // รอใส่ค่า
            this.PickUp = null;
            this.AirlineCode = dataTable2.airline_code.Trim();
            this.AirlineName = dataTable2.airline_desc.Trim();
            this.Time = new FlightTime();
            this.Time.Time24 = dataTable1.arrdep_time.Trim();
            TimeSpan timespan = TimeSpan.Parse(dataTable1.arrdep_time.Trim());
            DateTime time = DateTime.Today.Add(timespan);
            this.Time.TimeAMPM = time.ToString("hh:mm tt"); // It will give "03:00 AM"
            this.WeekDays = new List<FlightWeekDays>();
            if (dataTable1.d2)
            {
                var d2 = new FlightWeekDays();
                d2.ShortName = "MON";
                d2.FullName = ServiceModel.WeekDays.Monday.ToString();
                this.WeekDays.Add(d2);
            }

            if (dataTable1.d3)
            {
                var d3 = new FlightWeekDays();
                d3.ShortName = "TUE";
                d3.FullName = ServiceModel.WeekDays.Tuesday.ToString();
                this.WeekDays.Add(d3);
            }

            if (dataTable1.d4)
            {
                var d4 = new FlightWeekDays();
                d4.ShortName = "WED";
                d4.FullName = ServiceModel.WeekDays.Wednesday.ToString();
                this.WeekDays.Add(d4);
            }

            if (dataTable1.d5)
            {
                var d5 = new FlightWeekDays();
                d5.ShortName = "THU";
                d5.FullName = ServiceModel.WeekDays.Thursday.ToString();
                this.WeekDays.Add(d5);
            }

            if (dataTable1.d6)
            {
                var d6 = new FlightWeekDays();
                d6.ShortName = "FRI";
                d6.FullName = ServiceModel.WeekDays.Friday.ToString();
                this.WeekDays.Add(d6);
            }

            if (dataTable1.d7)
            {
                var d7 = new FlightWeekDays();
                d7.ShortName = "STU";
                d7.FullName = ServiceModel.WeekDays.Saturday.ToString();
                this.WeekDays.Add(d7);
            }

            if (dataTable1.d7)
            {
                var d1 = new FlightWeekDays();
                d1.ShortName = "SUN";
                d1.FullName = ServiceModel.WeekDays.Sunday.ToString();
                this.WeekDays.Add(d1);
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

    [DataContract]
    public class PickUp
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Detail { get; set; }

        [DataMember]
        public string MapImg { get; set; }

    }
}
