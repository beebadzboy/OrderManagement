using System;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace KP.OrderMGT.API.Authen
{
    public class BasicAuthenticationAttribute: AuthorizationFilterAttribute
    {
        private const string Realm = "My Authen";
        private const string key = "8b'rk;g;viN12345";
        //MODE: ECB
        //Keysite 128 bin( secreate 16 digit)

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //If the Authorization header is empty or null
            //then return Unauthorized
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request
                    .CreateResponse(HttpStatusCode.Unauthorized);

                // If the request was unauthorized, add the WWW-Authenticate header 
                // to the response which indicates that it require basic authentication
                if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    actionContext.Response.Headers.Add("WWW-Authenticate",
                        string.Format("Bearer realm=\"{0}\"", Realm));
                }
            }
            else
            {
                //Get the authentication token from the request header
                string authenticationToken = actionContext.Request.Headers
                    .Authorization.Parameter;

                //Decode the string
                //string decodedAuthenticationToken = Encoding.UTF8.GetString(
                //    Convert.FromBase64String(authenticationToken));
            

                if (UserValidate.Login(authenticationToken))
                {
                    //var identity = new GenericIdentity(authenticationToken);

                    //IPrincipal principal = new GenericPrincipal(identity, null);
                    //Thread.CurrentPrincipal = principal;

                    //if (HttpContext.Current != null)
                    //{
                    //    HttpContext.Current.User = principal;
                    //}

                    var UserDetails = UserValidate.GetUserDetails(authenticationToken);
                    var identity = new GenericIdentity(authenticationToken);
                    identity.AddClaim(new Claim("Email", UserDetails.UserEmailID));
                    identity.AddClaim(new Claim(ClaimTypes.Name, UserDetails.UserName));
                    identity.AddClaim(new Claim("ID", Convert.ToString(UserDetails.UserID)));

                    IPrincipal principal = new GenericPrincipal(identity, UserDetails.UserRoles.Split(','));
                    Thread.CurrentPrincipal = principal;
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = principal;
                    }
                }
                else {
                    actionContext.Response = actionContext.Request
                        .CreateResponse(HttpStatusCode.Unauthorized);
                }


                ////Convert the string into an string array
                //string[] usernamePasswordArray = decodedAuthenticationToken.Split(':');

                ////First element of the array is the username
                //string username = usernamePasswordArray[0];

                ////Second element of the array is the password
                //string password = usernamePasswordArray[1];

                ////call the login method to check the username and password
                //if (UserValidate.Login(username, password))
                //{
                //    //var identity = new GenericIdentity(username);

                //    //IPrincipal principal = new GenericPrincipal(identity, null);
                //    //Thread.CurrentPrincipal = principal;

                //    //if (HttpContext.Current != null)
                //    //{
                //    //    HttpContext.Current.User = principal;
                //    //}

                //    var UserDetails = UserValidate.GetUserDetails(username, password);
                //    var identity = new GenericIdentity(username);
                //    identity.AddClaim(new Claim("Email", UserDetails.UserEmailID));
                //    identity.AddClaim(new Claim(ClaimTypes.Name, UserDetails.UserName));
                //    identity.AddClaim(new Claim("ID", Convert.ToString(UserDetails.UserID)));

                //    IPrincipal principal = new GenericPrincipal(identity, UserDetails.UserRoles.Split(','));
                //    Thread.CurrentPrincipal = principal;
                //    if (HttpContext.Current != null)
                //    {
                //        HttpContext.Current.User =  principal;
                //    }
                //}
                //else
                //{
                //    actionContext.Response = actionContext.Request
                //        .CreateResponse(HttpStatusCode.Unauthorized);
                //}
            }
        }
    }
}