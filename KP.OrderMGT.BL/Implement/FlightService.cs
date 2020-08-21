using KP.OrderMGT.BL;
using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
            if (data == null)
            {
                throw new ObjectNotFoundException(fight_code + " not found");
            }

            var data2 = _db.df_airlines.AsQueryable().ToList();
            string airline_substring = data.flight_code.Trim().Length > 3 ? data.flight_code.Trim().Substring(0, 3) : data.flight_code.Trim();
            var airline_data3 = data2.FirstOrDefault(x => x.airline_code.Trim() == airline_substring);
            if (airline_data3 == null)
            {
                airline_substring = data.flight_code.Trim().Length > 2 ? data.flight_code.Trim().Substring(0, 2) : data.flight_code.Trim();
                var airline_data2 = data2.FirstOrDefault(x => x.airline_code.Trim() == airline_substring);
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

        public Flight CheckFlights(string fight_code, string fight_date)
        {

            DateTime oDate = Convert.ToDateTime(fight_date);
            var week_day = oDate.DayOfWeek.ToString();

            Expression<Func<df_flight, bool>> leftexp = x => x.flight_code == fight_code;
            Expression<Func<df_flight, bool>> rightexp = x => true;
            if (week_day == "Monday")
            {
                rightexp = x => x.d1 == true;
            }
            else if (week_day == "Tuesday")
            {
                rightexp = x => x.d2 == true;
            }
            else if (week_day == "Wednesday")
            {
                rightexp = x => x.d3 == true;
            }
            else if (week_day == "Thursday")
            {
                rightexp = x => x.d4 == true;
            }
            else if (week_day == "Friday")
            {
                rightexp = x => x.d5 == true;
            }
            else if (week_day == "Saturday")
            {
                rightexp = x => x.d6 == true;
            }
            else if (week_day == "Sunday")
            {
                rightexp = x => x.d7 == true;
            }

            var newData = new Flight();
            var lambda = leftexp.AndAlso<df_flight>(rightexp);
            var data = _db.df_flights.FirstOrDefault(lambda);
            if (data == null)
            {
                throw new ObjectNotFoundException(fight_code + " and " + week_day + " not found");
            }
            var data2 = _db.df_airlines.AsQueryable().ToList();
            string airline_substring = data.flight_code.Trim().Length > 3 ? data.flight_code.Trim().Substring(0, 3) : data.flight_code.Trim();
            var airline_data3 = data2.FirstOrDefault(x => x.airline_code.Trim() == airline_substring);
            if (airline_data3 == null)
            {
                airline_substring = data.flight_code.Trim().Length > 2 ? data.flight_code.Trim().Substring(0, 2) : data.flight_code.Trim();
                var airline_data2 = data2.FirstOrDefault(x => x.airline_code.Trim() == airline_substring);
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
            var airline_data3 = data2.FirstOrDefault(x => x.airline_code.Trim() == airline_substring);
            if (airline_data3 == null)
            {
                airline_substring = data.flight_code.Trim().Length > 2 ? data.flight_code.Trim().Substring(0, 2) : data.flight_code.Trim();
                var airline_data2 = data2.FirstOrDefault(x => x.airline_code.Trim() == airline_substring);
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
                var airline_data3 = data2.FirstOrDefault(x => x.airline_code.Trim() == airline_substring);
                if (airline_data3 == null)
                {
                    airline_substring = item.flight_code.Trim().Length > 2 ? item.flight_code.Trim().Substring(0, 2) : item.flight_code.Trim();
                    var airline_data2 = data2.FirstOrDefault(x => x.airline_code.Trim() == airline_substring);
                    if (airline_data2 != null)
                    {
                        newData = new Flight(item, airline_data2);
                    }
                }
                else
                {
                    newData = new Flight(item, airline_data3);
                }

                if (newData.Terminal == "A")
                {
                    ret.Arrival.Add(newData);
                }
                else if (newData.Terminal == "D")
                {
                    ret.Departure.Add(newData);
                }
                else if (newData.Terminal == "Z")
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

    public static class MyExtensions
    {
        public static IEnumerable<T> OrderByWithPropertyName<T>(this IEnumerable<T> source, string propertyName, ListSortDirection sortDirection)
        {
            if (!source.Any() || string.IsNullOrEmpty(propertyName))
                return source;

            var propertyInfo = source.First().GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (sortDirection == ListSortDirection.Ascending)
            {
                return source.OrderBy(e => propertyInfo.GetValue(e, null));
            }

            return source.OrderByDescending(e => propertyInfo.GetValue(e, null));
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }

        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }

        public static Expression<Func<T, bool>> PropertyEquals<T, TValue>(PropertyInfo property, TValue value)
        {
            var param = Expression.Parameter(typeof(T));
            var body = Expression.Equal(Expression.Property(param, property), Expression.Constant(value));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
