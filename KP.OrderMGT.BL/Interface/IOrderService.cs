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


    }


}
