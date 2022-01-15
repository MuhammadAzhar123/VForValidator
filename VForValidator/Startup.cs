using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Linq;
using VForValidator.DB;
using VForValidator.Extensions;

[assembly: OwinStartupAttribute(typeof(VForValidator.Startup))]
namespace VForValidator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            FirstInitSetup();
        }

        public void FirstInitSetup()
        {
            try
            {
                using(ApplicationDbContext _db = new ApplicationDbContext())
                {
                    if(!_db.Roles.Any(x => x.Name == Constants.Roles.ApplicationAdmin))
                    {
                        _db.Roles.Add(new IdentityRole(Constants.Roles.ApplicationAdmin));
                        _db.SaveChanges();
                    }
                    if (!_db.Roles.Any(x => x.Name == Constants.Roles.ApplicationUser))
                    {
                        _db.Roles.Add(new IdentityRole(Constants.Roles.ApplicationUser));
                        _db.SaveChanges();
                    }

                    if (!_db.Users.Any(u => u.UserName == "appadmin"))
                    {
                        var userStore = new UserStore<ApplicationUser>(_db);
                        var userManager = new ApplicationUserManager(userStore);

                        var roleStore = new RoleStore<IdentityRole>(_db);
                        var roleManager = new RoleManager<IdentityRole>(roleStore);
                        var user = new ApplicationUser()
                        {
                            UserName = "appadmin",
                            FirstName = "Application Admin",
                            LastName = "VForValidator",
                            RecordAt = System.DateTime.Now,
                            DisableDate = null,
                            Email = "mazharmafzal@gmail.com",
                            Code = "PasswordVFor".ToEncrypt()
                        };
                        userManager.Create(user, "PasswordVFor");
                        userManager.AddToRole(user.Id, Constants.Roles.ApplicationAdmin);
                        _db.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
