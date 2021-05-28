using System;
using mcg_load.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(mcg_load.Startup))]
namespace mcg_load
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            using (ApplicationDbContext dbContext = new ApplicationDbContext())
            {
                if (dbContext.Database.Exists())
                {
                    createRolesandUsers();
                }
                else
                {
                    Console.Out.WriteLine("No Hay Conexión");
                }
            }
            
        }

        // In this method we will create default User roles and Admin user for login    
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("SysAdmin"))
            {

                // first we create Admin rool    
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "SysAdmin";
                roleManager.Create(role);
            }

            // creating Creating User role     
            if (!roleManager.RoleExists("User"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

            }

            // creating Creating Manager role     
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            var user01 = UserManager.FindByName("sys");
            if (UserManager.FindByName("sys") == null)
            {
                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "sys";
                user.Email = "sys@gmail.com";

                string userPWD = "mQ5FCX.vFRKw&38";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "SysAdmin");

                }
            }
            


            if (UserManager.FindByName("raul.vanderhenst.ext") == null)
            {
                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser();
                user.UserName = "raul.vanderhenst.ext";
                user.Email = "raul.vanderhenst@itdesign.mcguate.com";

                string userPWD = "mQXSDCX.vFRKw&41";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "SysAdmin");

                }
            }
        }
    }
}
