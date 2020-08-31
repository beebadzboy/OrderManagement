using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace KP.OrderMGT.Service
{

    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        string GetConnectionPOSAirport(string airport_code);

        [OperationContract]
        string GetConnectionPOSOrder(string order_no);

        [OperationContract]
        SaleAmountByPassport ValidateAllowSaleOnline(POSAirPortClassesDataContext _posDB, string passort, DateTime flight_date, string flight_code);

        [OperationContract]
        OrderSession SaveOrderOnline(POSAirPortClassesDataContext _posDB, OrderHeader order);

        [OperationContract]
        OrderSession HoleOrderOnline(POSAirPortClassesDataContext _posDB, string order);

        [OperationContract]
        OrderSession CancelOrderOnline(POSAirPortClassesDataContext _posDB, string order_no);

        [OperationContract]
        OrderSession VoidOrderOnline(POSAirPortClassesDataContext _posDB, string order_no);

        [OperationContract]
        OrderSession CompleteOrderOnline(POSAirPortClassesDataContext _posDB, string order_no);

        [OperationContract]
        OrderSession GetOrderOnline(string order_no);

        [OperationContract]
        List<OrderSession> GetOrderOnlineList(string airport_code, int? skip, int? take);

        [OperationContract]
        List<SaleQueue> SaleQueue(POSAirPortClassesDataContext _posDB, char terminal);

        [OperationContract]
        OrderSession UpdateStatusOrderOnline(string order_no, string status);
    }


}
