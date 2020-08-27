using KP.Common.Return;
using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using KP.OrderMGT.Service;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;

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

        [Authorize(Roles = "SuperAdmin, Admin, User")]
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

                    ret.totalCount = ret.Data != null ? 1 : 0;
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

            return Ok(ret);
        }

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpPost]
        [Route("save-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public IHttpActionResult SaveOrderOnline(OrderHeader order)
        {
            // validate data
            /////

            ReturnObject<OrderSession> ret = new ReturnObject<OrderSession>();
            try
            {
                var omSrv = new OrderService(omDB);
                var posConn = omSrv.GetConnectionPOSAirport(order.Flight.AirportCode);
                if (posConn != null)
                {
                    posDB = new POSAirPortClassesDataContext(posConn);
                    ret.Data = omSrv.SaveOrderOnline(posDB, order);

                    ret.totalCount = ret.Data != null ? 1 : 0;
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

            return Ok(ret);
        }

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("hold-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public IHttpActionResult HoldOrderOnline(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                throw new ArgumentException("message", nameof(order_no));
            }

            ReturnObject<OrderSession> ret = new ReturnObject<OrderSession>();
            try
            {
                var omSrv = new OrderService(omDB);
                var posConn = omSrv.GetConnectionPOSOrder(order_no);
                if (posConn != null)
                {
                    posDB = new POSAirPortClassesDataContext(posConn);
                    ret.Data = omSrv.HoleOrderOnline(posDB, order_no);

                    ret.totalCount = ret.Data != null ? 1 : 0;
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

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("cancel-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public IHttpActionResult CancelOrderOnline(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                throw new ArgumentException("message", nameof(order_no));
            }

            ReturnObject<OrderSession> ret = new ReturnObject<OrderSession>();

            try
            {
                var omSrv = new OrderService(omDB);
                var posConn = omSrv.GetConnectionPOSOrder(order_no);
                if (posConn != null)
                {
                    posDB = new POSAirPortClassesDataContext(posConn);
                    ret.Data = omSrv.CancelOrderOnline(posDB, order_no);

                    ret.totalCount = ret.Data != null ? 1 : 0;
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


            return Ok(ret);
        }

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("get-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public IHttpActionResult GetOrderOnline(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                throw new ArgumentException("message", nameof(order_no));
            }

            ReturnObject<OrderSession> ret = new ReturnObject<OrderSession>();

            try
            {
                var omSrv = new OrderService(omDB);
                ret.Data = omSrv.GetOrderOnline(order_no);
                ret.totalCount = ret.Data != null? 1 : 0;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
                ret.Tracking = new ReturnTracking();
            }


            return Ok(ret);
        }

        [Authorize(Roles = "SuperAdmin, Admin, User")]
        [HttpGet]
        [Route("get-order-list")]
        [ResponseType(typeof(ReturnObject<List<OrderSession>>))]
        public IHttpActionResult GetOrderOnlineList(string airport_code, int? skip, int? take)
        {
            if (string.IsNullOrWhiteSpace(airport_code))
            {
                throw new ArgumentException("message", nameof(airport_code));
            }

            ReturnObject<List<OrderSession>> ret = new ReturnObject<List<OrderSession>>();

            try
            {
                var omSrv = new OrderService(omDB);
                ret.Data = omSrv.GetOrderOnlineList(airport_code, skip,take);

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

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPut]
        //[Obsolete]
        [Route("complate-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public async Task<IHttpActionResult> ComplateOrderOnlineAsync(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                throw new ArgumentException("message", nameof(order_no));
            }

            ReturnObject<OrderSession> ret = new ReturnObject<OrderSession>();

            try
            {
                var omSrv = new OrderService(omDB);
                ret.Data = omSrv.ComplateOrderOnline(order_no);
                if (ret.Data != null)
                {
                    // send update to endpoint COMPLETED 
                    var client = new RestClient("http://10.3.26.32:5000");
                    var request = new RestRequest(String.Format("dev/api/Orders/{0}/Status", order_no), Method.POST);
                    request.AddHeader("AccessToken", "A64803F0A7CEDAC8407538D341BDBE23");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddJsonBody(new { statuscode = "receivecomplete" });
                    var restResponse = await client.ExecutePostTaskAsync(request);
                    if (restResponse.ErrorException != null)
                    {
                        throw restResponse.ErrorException.InnerException;
                    }
                    else
                    {
                        if (restResponse.StatusCode != HttpStatusCode.OK)
                        {
                            ret.Data = omSrv.UpdateStatusOrderOnline(order_no, restResponse.StatusCode.ToString());
                            ret.totalCount = 0;
                            ret.isCompleted = false;
                        }
                        else
                        {
                            ret.Data = omSrv.UpdateStatusOrderOnline(order_no, "receivecomplete");
                            ret.totalCount = 1;
                            ret.isCompleted = true;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("message", "connection error");
                }


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

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPut]
        //[Obsolete]
        [Route("void-order")]
        [ResponseType(typeof(ReturnObject<OrderSession>))]
        public async Task<IHttpActionResult> VoidOrderOnlineAsync(string order_no)
        {
            if (string.IsNullOrWhiteSpace(order_no))
            {
                throw new ArgumentException("message", nameof(order_no));
            }

            ReturnObject<OrderSession> ret = new ReturnObject<OrderSession>();

            try
            {
                var omSrv = new OrderService(omDB);
                ret.Data = omSrv.VoidOrderOnline(order_no);
                ret.Data = new OrderSession();
                if (ret.Data != null)
                {
                    // send update to endpoint CANCELED 
                    var client = new RestClient("http://10.3.26.32:5000");
                    var request = new RestRequest(String.Format("dev/api/Orders/{0}/Status", order_no), Method.POST);
                    request.AddHeader("AccessToken", "A64803F0A7CEDAC8407538D341BDBE23");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddJsonBody(new { statuscode = "refund" });
                    var restResponse = await client.ExecutePostTaskAsync(request);
                    if (restResponse.ErrorException != null)
                    {
                        throw restResponse.ErrorException.InnerException;
                    }
                    else
                    {
                        if (restResponse.StatusCode != HttpStatusCode.OK)
                        {
                            ret.Data = omSrv.UpdateStatusOrderOnline(order_no, restResponse.StatusCode.ToString());
                            ret.totalCount = 0;
                            ret.isCompleted = false;
                        }
                        else
                        {
                            ret.Data = omSrv.UpdateStatusOrderOnline(order_no, "refund");
                            ret.totalCount = 1;
                            ret.isCompleted = true;
                        }
                    }
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

            return Ok(ret);
        }
    }
}
