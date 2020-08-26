using KP.OrderBusiness.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace KP.OrderBusiness.ServiceModel
{

    [DataContract]
    public enum WeekDays
    {
        Monday = 2,
        Tuesday = 3,
        Wednesday = 4,
        Thursday = 5,
        Friday = 6,
        Saturday = 7,
        Sunday = 1
    }

    [DataContract]
    public enum Terminal
    {
        Departure = 1,  // ออก
        Arrival = 2,    // เข้า
        Tranfer = 3     // เปลี่ยน  
    }

}
