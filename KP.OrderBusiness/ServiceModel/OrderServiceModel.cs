using KP.OrderBusiness.DBModel;
using KP.OrderBusiness.Interface;
using KP.OrderBusiness.ServiceModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace KP.OrderBusiness.ServiceModel
{

    [DataContract]
    public enum StatusOrderPOS
    {
        Created = 002,
        HoldOrder = 0025,
        CancelCreated = 0021,
        RefundComplete = 103,
        Saved = 003,
        Complete = 006
    }



}
