using KP.OrderMGT.BL.ServiceModel;
using System.Collections.Generic;
using System.ServiceModel;

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
