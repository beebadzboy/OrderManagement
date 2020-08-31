using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KP.OrderMGT.API.Authen
{
    public class UserValidate
    {

        //This method is used to check the user credentials
        public static bool Login(string username, string password)
        {
            //UsersBL userBL = new UsersBL();

            //var UserLists = userBL.GetUsers();
            //return UserLists.Any(user =>
            //    user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
            //    && user.Password == password);

            KPOrderEntities context = new KPOrderEntities();

            var UserLists = context.user_master.FirstOrDefault(user => user.UserName == username && user.UserPassword == password);

            return UserLists != null ? true : false;

        }

        public static bool Login(string user_token)
        {
            //UsersBL userBL = new UsersBL();

            //var UserLists = userBL.GetUsers();
            //return UserLists.Any(user =>
            //    user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
            //    && user.Password == password);

            KPOrderEntities context = new KPOrderEntities();

            var UserLists = context.user_master.FirstOrDefault(user => user.UserToken == user_token);

            return UserLists != null ? true : false;

        }

        public static user_master GetUserDetails(string username, string password)
        {
            KPOrderEntities context = new KPOrderEntities();
            var UserLists = context.user_master.FirstOrDefault(user => user.UserName == username && user.UserPassword == password);

            return UserLists;

        }


        public static user_master GetUserDetails(string user_token)
        {
            KPOrderEntities context = new KPOrderEntities();
            var UserLists = context.user_master.FirstOrDefault(user => user.UserToken == user_token);

            return UserLists;

        }


    }
}