using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mcg_load.Models;

namespace mcg_load.Code
{

    public class ApplicationUserView
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
    }

    public static class AuthHelper {
  

        public static bool SignIn(string userName, string password)
        {
            HttpContext.Current.Session["User"] = CreateDefualtUser();  // Mock user data
            return true;
        }

        public static void SignOut()
        {
            HttpContext.Current.Session["User"] = null;
        }
        public static bool IsAuthenticated()
        {
            return GetLoggedInUserInfo() != null;
        }

        public static ApplicationUserView GetLoggedInUserInfo()
        {
            return HttpContext.Current.Session["User"] as ApplicationUserView;
        }
        private static ApplicationUserView CreateDefualtUser()
        {
            return new ApplicationUserView
            {
                UserName = "JBell",
                FirstName = "Julia",
                LastName = "Bell",
                Email = "julia.bell@example.com",
                AvatarUrl = "~/Content/Photo/Julia_Bell.jpg"
            };
        }
    }
}