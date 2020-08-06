using KP.OrderMGT.BL;
using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using System.Collections.Generic;
using System.Linq;

namespace KP.OrderMGT.Service
{
    public class FlightService : IFlightService
    {
        private OrderDataClassesDataContext _db;
        public FlightService(OrderDataClassesDataContext db)
        { _db = db; }

        public Flight CheckFlights(string fight_code)
        {
            var newData = new Flight();
            var data = _db.df_flights.FirstOrDefault(x => x.flight_code == fight_code);
            var data2 = _db.df_airlines.AsQueryable().ToList();
            string airline_substring = data.flight_code.Trim().Length > 3 ? data.flight_code.Trim().Substring(0, 3) : data.flight_code.Trim();
            var airline_data3 = data2.FirstOrDefault(x => x.airline_code == airline_substring);
            if (airline_data3 == null)
            {
                airline_substring = data.flight_code.Trim().Length > 2 ? data.flight_code.Trim().Substring(0, 2) : data.flight_code.Trim();
                var airline_data2 = data2.FirstOrDefault(x => x.airline_code == airline_substring);
                if (airline_data2 != null)
                {
                    newData = new Flight(data, airline_data2);
                }
            }
            else
            {
                newData = new Flight(data, airline_data3);
            }


            return newData;
        }

        public Flight GetDataFlights(string fight_code)
        {
            var newData = new Flight();
            var data = _db.df_flights.FirstOrDefault(x => x.flight_code == fight_code);
            var data2 = _db.df_airlines.AsQueryable().ToList();
            string airline_substring = data.flight_code.Trim().Length > 3 ? data.flight_code.Trim().Substring(0, 3) : data.flight_code.Trim();
            var airline_data3 = data2.FirstOrDefault(x => x.airline_code == airline_substring);
            if (airline_data3 == null)
            {
                airline_substring = data.flight_code.Trim().Length > 2 ? data.flight_code.Trim().Substring(0, 2) : data.flight_code.Trim();
                var airline_data2 = data2.FirstOrDefault(x => x.airline_code == airline_substring);
                if (airline_data2 != null)
                {
                    newData = new Flight(data, airline_data2);
                }
            }
            else
            {
                newData = new Flight(data, airline_data3);
            }


            return newData;
        }

        public FlightsAll GetDataAll()
        {
            FlightsAll ret = new FlightsAll();
            ret.Arrival = new List<Flight>();
            ret.Departure = new List<Flight>();
            ret.Transfer = new List<Flight>();
            var data = _db.df_flights.Where(x => x.arrdep_terminal != "L" && x.arrdep_terminal != "Y" && x.arrdep_terminal != "").AsQueryable();
            var data2 = _db.df_airlines.AsQueryable().ToList();
            foreach (var item in data.ToList())
            {
                var newData = new Flight();
                string airline_substring = item.flight_code.Trim().Length > 3 ? item.flight_code.Trim().Substring(0, 3) : item.flight_code.Trim();
                var airline_data3 = data2.FirstOrDefault(x => x.airline_code == airline_substring);
                if (airline_data3 == null)
                {
                    airline_substring = item.flight_code.Trim().Length > 2 ? item.flight_code.Trim().Substring(0, 2) : item.flight_code.Trim();
                    var airline_data2 = data2.FirstOrDefault(x => x.airline_code == airline_substring);
                    if (airline_data2 != null)
                    {
                        newData = new Flight(item, airline_data2);
                    }
                }
                else
                {
                    newData = new Flight(item, airline_data3);
                }

                if (newData.Teminal == "A")
                {
                    ret.Arrival.Add(newData);
                }
                else if (newData.Teminal == "D")
                {
                    ret.Departure.Add(newData);
                }
                else if (newData.Teminal == "Z")
                {
                    ret.Transfer.Add(newData);
                }
            }

            return ret;
        }

        public List<Flight> GetDataArrival()
        {
            var data = GetDataAll();
            return data.Arrival;
        }

        public List<Flight> GetDataDeparture()
        {
            var data = GetDataAll();
            return data.Departure;
        }

        public List<Flight> GetDataTransfer()
        {
            var data = GetDataAll();
            return data.Transfer;
        }


    }
}
