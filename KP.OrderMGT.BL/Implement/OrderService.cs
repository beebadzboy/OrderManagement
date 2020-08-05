using KP.Common.Return;
using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
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
        public SaleOnlineByPassport ValidateAllowSaleOnline(POSAirPortClassesDataContext _posDB, string airport_code, string fight_code, string passort, string date, string time)
        {
            throw new System.NotImplementedException();
        }
    }
}
