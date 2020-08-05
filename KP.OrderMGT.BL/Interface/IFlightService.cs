using KP.Common.Return;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using KP.OrderMGT.BL.ServiceModel;

namespace KP.OrderMGT.BL
{
    [ServiceContract]
    public interface IFlightService
    {

        [OperationContract]
        Flight CheckFlights(string fight_code);

        [OperationContract]
        Flight GetDataFlights(string fight_code);

        [OperationContract]
        FlightsAll GetDataAll();

        [OperationContract]
        List<Flight> GetDataDeparture();

        [OperationContract]
        List<Flight> GetDataArrival();

        [OperationContract]
        List<Flight> GetDataTransfer();
    }

}
