using KP.Common.Return;
using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using KP.OrderMGT.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace KP.OrderMGT.API.Controllers
{
    [RoutePrefix("api/queue")]
    public class SaleQueueController : ApiController
    {
        public OrderDataClassesDataContext orderDB { get; set; }
        public SaleQueueController()
        {
            string connStr = ConfigurationManager.ConnectionStrings["OrderConnectionString"].ConnectionString;
            orderDB = new OrderDataClassesDataContext(connStr);
        }

        [HttpGet]
        [Route("sale-by-sku")]
        [ResponseType(typeof(ReturnObject<List<SaleQueue>>))]
        public IHttpActionResult SaleQueueOnline(string airport_code, char terminal)
        {

            ReturnObject<List<SaleQueue>> ret = new ReturnObject<List<SaleQueue>>();

            try
            {
                if (string.IsNullOrWhiteSpace(airport_code))
                {
                    throw new ArgumentException("message", nameof(airport_code));
                }

                if (char.IsWhiteSpace(terminal))
                {
                    throw new ArgumentException("message", nameof(terminal));
                }

                var omSrv = new OrderService(orderDB);
                var posConn = omSrv.GetConnectionPOSAirport(airport_code);
                if (posConn != null)
                {
                    var posDB = new POSAirPortClassesDataContext(posConn);
                    ret.Data = omSrv.SaleQueue(posDB, terminal);
                    ret.totalCount = ret.Data.Count();
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
    }
}
