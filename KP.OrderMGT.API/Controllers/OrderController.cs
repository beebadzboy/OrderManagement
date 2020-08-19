using KP.Common.Return;
using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using KP.OrderMGT.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Http;
using System.Web.Http.Description;

namespace KP.OrderMGT.API.Controllers
{
    [RoutePrefix("api/online")]
    public class OrderController : ApiController
    {
        public OrderDataClassesDataContext omDB { get; set; }
        public POSAirPortClassesDataContext posDB { get; set; }
        public OrderController()
        {
            string connStr = ConfigurationManager.ConnectionStrings["OrderConnectionString"].ConnectionString;
            omDB = new OrderDataClassesDataContext(connStr);
        }

        [HttpGet]
        [Route("check-purchase-rights")]
        [ResponseType(typeof(ReturnObject<SaleAmountByPassport>))]
        public IHttpActionResult CheckAllowSaleOnline(string airport_code, char terminal, string passport, string date, int time)
        {
            ReturnObject<SaleAmountByPassport> ret = new ReturnObject<SaleAmountByPassport>();
            try
            {

                if (string.IsNullOrEmpty(airport_code))
                {
                    throw new ArgumentException("message", nameof(airport_code));
                }

                if (char.IsWhiteSpace(terminal))
                {
                    throw new ArgumentException("message", nameof(passport));
                }

                DateTime dateTime = Convert.ToDateTime(date);
                if (string.IsNullOrEmpty(date))
                {
                    throw new ArgumentException("message", nameof(date));
                }
                else
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    dateTime = DateTime.ParseExact(date, "yyyy-MM-dd", provider);
                }

                var omSrv = new OrderService(omDB);
                var posConn = omSrv.GetConnectionPOSAirport(airport_code);
                if (posConn != null)
                {
                    posDB = new POSAirPortClassesDataContext(posConn);
                    ret.Data = omSrv.ValidateAllowSaleOnline(posDB, terminal, passport, dateTime, time);

                    ret.totalCount = 1;
                    ret.isCompleted = true;
                }
                else
                {
                    throw new ArgumentException("message", "connection error");
                }

            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }

            return Ok(ret.Data);
        }

        [HttpPost]
        [Route("save-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public IHttpActionResult SaveOrderOnline(OrderHeader order)
        {
            ReturnObject<OrderHeader> ret = new ReturnObject<OrderHeader>();
            //ret.Data = order;

            try
            {
                var omSrv = new OrderService(omDB);
                var omConn = omSrv.GetConnectionPOSAirport(order.Flight.AirportCode);

                ret.totalCount = 1;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }

            return Ok(ret.Data);
        }

        [HttpGet]
        [Route("hold-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public IHttpActionResult HoldOrderOnline(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                throw new ArgumentException("message", nameof(order_no));
            }

            ReturnObject<string> ret = new ReturnObject<string>();
            ret.Data = order_no;

            try
            {
                var omSrv = new OrderService(omDB);

                ret.totalCount = 1;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }


            return Ok(ret.Data);
        }

        [HttpPost]
        [Route("hold-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public IHttpActionResult HoldOrderOnline(OrderHeader order)
        {
            ReturnObject<OrderHeader> ret = new ReturnObject<OrderHeader>();
            ret.Data = order;

            try
            {
                var omSrv = new OrderService(omDB);
                var omConn = omSrv.GetConnectionPOSAirport(order.Flight.AirportCode);

                ret.totalCount = 1;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }


            return Ok(ret.Data);
        }

        [HttpPut]
        [Route("void-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public IHttpActionResult VoidOrderOnline(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                throw new ArgumentException("message", nameof(order_no));
            }

            ReturnObject<string> ret = new ReturnObject<string>();
            ret.Data = order_no;

            try
            {
                var omSrv = new OrderService(omDB);

                ret.totalCount = 1;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }


            return Ok(ret.Data);
        }

        [HttpGet]
        [Route("get-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public IHttpActionResult GetOrderOnline(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                throw new ArgumentException("message", nameof(order_no));
            }

            ReturnObject<OrderHeader> ret = new ReturnObject<OrderHeader>();

            try
            {
                var omSrv = new OrderService(omDB);

                ret.totalCount = 1;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }


            return Ok(ret.Data);
        }

        [HttpGet]
        [Route("get-order-list")]
        [ResponseType(typeof(ReturnObject<List<OrderSession>>))]
        public IHttpActionResult GetOrderOnlineList(string airport_code, int? skip, int? take)
        {
            if (string.IsNullOrWhiteSpace(airport_code))
            {
                throw new ArgumentException("message", nameof(airport_code));
            }

            ReturnObject<List<OrderHeader>> ret = new ReturnObject<List<OrderHeader>>();

            try
            {
                var omSrv = new OrderService(omDB);

                ret.totalCount = 1;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }

            return Ok(ret.Data);
        }


    }
}
