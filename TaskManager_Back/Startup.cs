using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using TaskManager_Back.Models;

[assembly: OwinStartup(typeof(TaskManager_Back.Startup))]

namespace TaskManager_Back
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUserAndRoles();
        }

        public void CreateUserAndRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("admin"))
            {
                //create super admin role
                var role = new IdentityRole("admin");
                roleManager.Create(role);


                //create default user

                var admin = userManager.FindByEmail("admin@example.com");
                if (admin != null)
                {
                    userManager.AddToRole(admin.Id, "admin");
                }

            }
            if (!roleManager.RoleExists("user"))
            {
                var role = new IdentityRole("user");
                roleManager.Create(role);
                
            }
        }
    }
}
