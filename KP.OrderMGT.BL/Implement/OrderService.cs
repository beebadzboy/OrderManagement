using KP.Common.Return;
using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using System;
using System.Linq;
using System.Xml;

namespace KP.OrderMGT.Service
{
    public class OrderService : IOrderService
    {
        private OrderDataClassesDataContext _omDB;
        private POSAirPortClassesDataContext _posDB;
        public OrderService() { }
        public OrderService(OrderDataClassesDataContext omDB)
        { _omDB = omDB; }
        public OrderService(POSAirPortClassesDataContext posDB)
        { _posDB = posDB; }
        public OrderService(OrderDataClassesDataContext omDB, POSAirPortClassesDataContext posDB)
        { _omDB = omDB; _posDB = posDB; }

        public string GetConnectionPOSAirport(string airport_code)
        {
            var connObj = _omDB.config_connections.FirstOrDefault(x => x.cn_code == airport_code);
            if (connObj != null)
            {
                throw new System.ArgumentException("message", nameof(connObj));
            }

            var connectionString = "Data Source="+ connObj.cn_server + ";Initial Catalog="+ connObj.cn_database + ";Persist Security Info=True;User ID=" + connObj.cn_uid + ";Password=" + connObj.cn_pwd + ";";
            return connectionString;
        }

        public SaleOnlineByPassport GetOrderOnline(string order_no)
        {
            throw new NotImplementedException();
        }

        public SaleOnlineByPassport GetOrderOnlineList(string airport_code, int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public SaleOnlineByPassport HoleOrderOnline(OrderHeader order)
        {
            throw new NotImplementedException();
        }

        public SaleOnlineByPassport HoleOrderOnline(string order_no)
        {
            throw new NotImplementedException();
        }

        public SaleOnlineByPassport SaveOrderOnline(POSAirPortClassesDataContext _posDB, OrderHeader order)
        {
            throw new NotImplementedException();
        }

        public SaleOnlineByPassport SaveSaleOnline(POSAirPortClassesDataContext _posDB, OrderHeader order)
        {
            throw new NotImplementedException();
        }

        public SaleOnlineByPassport ValidateAllowSaleOnline(POSAirPortClassesDataContext _posDB, char terminal, string passort, DateTime date, int time)
        {
            var connObj = _posDB.get_sale_passport4(time, passort, date, terminal);
            if (connObj != null)
            {
                throw new System.ArgumentException("message", nameof(connObj));
            }

            return new SaleOnlineByPassport();
        }

        public SaleOnlineByPassport VoidOrderOnline(string order_no)
        {
            throw new NotImplementedException();
        }
    }
}
