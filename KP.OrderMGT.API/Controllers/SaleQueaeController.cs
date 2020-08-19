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
    [RoutePrefix("api/sale")]
    public class SaleQueaeController : ApiController
    {
        public OrderDataClassesDataContext orderDB { get; set; }
        public SaleQueaeController()
        {
            string connStr = ConfigurationManager.ConnectionStrings["OrderConnectionString"].ConnectionString;
            orderDB = new OrderDataClassesDataContext(connStr);
        }

        [HttpGet]
        [Route("sale-by-sku")]
        [ResponseType(typeof(ReturnObject<List<SaleQueae>>))]
        public IHttpActionResult SaveOrderOnline([FromBody] string terminal)
        {
            ReturnObject<List<SaleQueae>> ret = new ReturnObject<List<SaleQueae>>();

            try
            {
                var omSrv = new OrderService(orderDB);
                //var omConn = omSrv.GetConnectionPOSAirport(order.Flight.AirportCode);

                ret.totalCount = 1;
                ret.isCompleted = true;
            }
            catch (Exception e)
            {
                ret.SetMessage(e);
            }

            return Ok(ret.Data);
        }
    }
}
