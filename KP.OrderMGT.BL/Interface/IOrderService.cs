using KP.OrderMGT.BL.DBModel;
using KP.OrderMGT.BL.ServiceModel;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace KP.OrderMGT.Service
{

    [ServiceContract]
    public interface IOrderService
    {
        [OperationContract]
        string GetConnectionPOSAirport(string airport_code);

        [OperationContract]
        SaleOnlineByPassport ValidateAllowSaleOnline(POSAirPortClassesDataContext _posDB, char terminal, string passort, DateTime date, int time);

        [OperationContract]
        SaleOnlineByPassport SaveOrderOnline(POSAirPortClassesDataContext _posDB, OrderHeader order);

        [OperationContract]
        SaleOnlineByPassport HoleOrderOnline(OrderHeader order);

        [OperationContract]
        SaleOnlineByPassport HoleOrderOnline(string order_no);

        [OperationContract]
        SaleOnlineByPassport VoidOrderOnline(string order_no);

        [OperationContract]
        SaleOnlineByPassport GetOrderOnline(string order_no);

        [OperationContract]
        SaleOnlineByPassport GetOrderOnlineList(string airport_code, int? skip, int? take);
    }


}
