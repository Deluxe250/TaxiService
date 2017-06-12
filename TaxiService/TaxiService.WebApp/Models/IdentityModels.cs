using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System;

namespace TaxiService.WebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Orders = new HashSet<Order>();
        }

        public string FullName { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public virtual ICollection<Order> Orders { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new CustomContextInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderStatus> OrderStatuses { get; set; }
    }

    class CustomContextInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext ctx)
        {
            CreateRoles(ctx);
            CreateUsers(ctx);
            CreateOrderStatuses(ctx);
            base.Seed(ctx);
        }

        private void CreateRoles(ApplicationDbContext ctx)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));

            roleManager.Create(new IdentityRole(Roles.Admin.ToString()));
            roleManager.Create(new IdentityRole(Roles.User.ToString()));
            roleManager.Create(new IdentityRole(Roles.Dispatcher.ToString()));
            roleManager.Create(new IdentityRole(Roles.Driver.ToString()));
        }

        private void CreateUsers(ApplicationDbContext ctx)
        {
            var defaultPassword = "@Test1";
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(ctx));

            var admin = new ApplicationUser { FullName = "Александр Сергеевич Быстров", Email = "admin@t.ru", UserName = "admin@t.ru" };
            var user = new ApplicationUser { FullName = "Владислав Андреевич Воронин", Email = "user@t.ru", UserName = "user@t.ru" };
            var dispatcher = new ApplicationUser { FullName = "Елена Владимировна Янковская", Email = "dispatcher@t.ru", UserName = "dispatcher@t.ru" };
            var driver1 = new ApplicationUser { FullName = "Юрий Витальевич Комов", Email = "driver1@t.ru", UserName = "driver1@t.ru" };
            var driver2 = new ApplicationUser { FullName = "Игорь Николаевич Алёхин", Email = "driver2@t.ru", UserName = "driver2@t.ru" };
            var driver3 = new ApplicationUser { FullName = "Иван Григорьевич Самсонов", Email = "driver3@t.ru", UserName = "driver3@t.ru" };

            var result = userManager.Create(admin, defaultPassword);
            userManager.Create(user, defaultPassword);
            userManager.Create(dispatcher, defaultPassword);
            userManager.Create(driver1, defaultPassword);
            userManager.Create(driver2, defaultPassword);
            userManager.Create(driver3, defaultPassword);

            userManager.AddToRole(admin.Id, Roles.Admin.ToString());
            userManager.AddToRole(user.Id, Roles.User.ToString());
            userManager.AddToRole(dispatcher.Id, Roles.Dispatcher.ToString());
            userManager.AddToRole(driver1.Id, Roles.Driver.ToString());
            userManager.AddToRole(driver2.Id, Roles.Driver.ToString());
            userManager.AddToRole(driver3.Id, Roles.Driver.ToString());
        }

        private void CreateOrderStatuses(ApplicationDbContext ctx)
        {
            ctx.OrderStatuses.Add(new OrderStatus { Name = "Новая", Mnemonic = OrderStatuses.New.ToString() });
            ctx.OrderStatuses.Add(new OrderStatus { Name = "Выполняется", Mnemonic = OrderStatuses.InProgress.ToString() });
            ctx.OrderStatuses.Add(new OrderStatus { Name = "Выполнена", Mnemonic = OrderStatuses.Done.ToString() });
            ctx.OrderStatuses.Add(new OrderStatus { Name = "Отменена", Mnemonic = OrderStatuses.Cancelled.ToString() });
        }
    }
}