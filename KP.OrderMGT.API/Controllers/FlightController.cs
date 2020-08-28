using KP.Common.Return;
using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using KP.OrderMGT.Service;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace KP.OrderMGT.API.Controllers
{
    //[Authorize(Roles = "SuperAdmin, Admin, User")]
    [RoutePrefix("api/flight")]
    public class FlightController : ApiController
    {
        public OrderDataClassesDataContext orderDB { get; set; }
        public FlightController()
        {
            string connStr = ConfigurationManager.ConnectionStrings["OrderConnectionString"].ConnectionString;
            orderDB = new OrderDataClassesDataContext(connStr);
        }

        [HttpGet]
        [Route("validate-code")]
        [ResponseType(typeof(ReturnObject<Flight>))]
        public IHttpActionResult ValidateFlights(string flight_code)
        {
            ReturnObject<Flight> ret = new ReturnObject<Flight>();
            try
            {
                if (string.IsNullOrEmpty(flight_code))
                {
                    throw new ArgumentException("message", nameof(flight_code));
                }

                var srv = new FlightService(orderDB);
                ret.Data = srv.CheckFlights(flight_code);
                ret.totalCount = 1;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }


            return Ok(ret);
        }

        [HttpGet]
        [Route("check-flight")]
        [ResponseType(typeof(ReturnObject<Flight>))]
        public IHttpActionResult CheckFlights(string flight_code, string flight_date)
        {
            ReturnObject<Flight> ret = new ReturnObject<Flight>();
            try
            {
                if (string.IsNullOrEmpty(flight_code))
                {
                    throw new ArgumentException("flight code", nameof(flight_code));
                }

                if (string.IsNullOrEmpty(flight_date))
                {
                    throw new ArgumentException("flight date", nameof(flight_date));
                }


                var srv = new FlightService(orderDB);
                ret.Data = srv.CheckFlights(flight_code, flight_date);
                ret.totalCount = 1;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }


            return Ok(ret);
        }

        [HttpGet]
        [Route("all")]
        [ResponseType(typeof(ReturnObject<FlightsAll>))]
        public IHttpActionResult GetAll()
        {
            ReturnObject<FlightsAll> ret = new ReturnObject<FlightsAll>();
            try
            {
                var srv = new FlightService(orderDB);
                ret.Data = srv.GetDataAll();

                int d = ret.Data.Departure.Count;
                int a = ret.Data.Arrival.Count;
                int z = ret.Data.Transfer.Count;

                ret.totalCount = d + a + z;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                //ret.Tracking = new ReturnTracking();
            }


            return Ok(ret);
        }

        [HttpGet]
        [Route("arrival")]
        [ResponseType(typeof(ReturnObject<List<Flight>>))]
        public IHttpActionResult GetArrival()
        {
            ReturnObject<List<Flight>> ret = new ReturnObject<List<Flight>>();
            try
            {
                var srv = new FlightService(orderDB);
                ret.Data = srv.GetDataArrival();
                ret.totalCount = ret.Data.Count;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }

            return Ok(ret);

        }

        [HttpGet]
        [Route("departure")]
        [ResponseType(typeof(ReturnObject<List<Flight>>))]
        public IHttpActionResult GetDeparture()
        {
            ReturnObject<List<Flight>> ret = new ReturnObject<List<Flight>>();
            try
            {
                var srv = new FlightService(orderDB);
                ret.Data = srv.GetDataDeparture();
                ret.totalCount = ret.Data.Count;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }

            return Ok(ret);
        }

        [HttpGet]
        [Route("transfer")]
        [ResponseType(typeof(ReturnObject<List<Flight>>))]
        public IHttpActionResult GetTransfer()
        {
            ReturnObject<List<Flight>> ret = new ReturnObject<List<Flight>>();
            try
            {
                var srv = new FlightService(orderDB);
                ret.Data = srv.GetDataTransfer();
                ret.totalCount = ret.Data.Count;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }

            return Ok(ret);
        }


    }
}
