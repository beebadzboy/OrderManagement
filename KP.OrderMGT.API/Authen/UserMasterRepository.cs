using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KP.OrderMGT.API.Authen
{
    public class UserMasterRepository :IDisposable
    {
        // SECURITY_DBEntities it is your context class
        KPOrderEntities context = new KPOrderEntities();


        //This method is used to check and validate the user credentials
        public user_master ValidateUser(string username, string password)
        {
            return context.user_master.FirstOrDefault(user =>
            user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
            && user.UserPassword == password);
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}