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
        }

        private void CreateRoles(ApplicationDbContext ctx)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(ctx));

            roleManager.Create(new IdentityRole() { Name = Roles.Admin.ToString() });
            roleManager.Create(new IdentityRole() { Name = Roles.User.ToString() });
            roleManager.Create(new IdentityRole() { Name = Roles.Dispatcher.ToString() });
            roleManager.Create(new IdentityRole() { Name = Roles.Driver.ToString() });
        }

        private void CreateUsers(ApplicationDbContext ctx)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(ctx));

            var admin = new ApplicationUser { UserName = "Александр Сергеевич Быстров", Email = "admin@t.ru" };
            var user = new ApplicationUser { UserName = "Владислав Андреевич Воронин", Email = "user@t.ru" };
            var dispatcher = new ApplicationUser { UserName = "Елена Владимировна Янковская", Email = "dispatcher@t.ru" };
            var driver = new ApplicationUser { UserName = "Юрий Витальевич Комов", Email = "driver@t.ru" };

            ctx.Users.Add(admin);
            ctx.Users.Add(user);
            ctx.Users.Add(dispatcher);
            ctx.Users.Add(driver);

            userManager.AddToRole(admin.Id, Roles.Admin.ToString());
            userManager.AddToRole(user.Id, Roles.User.ToString());
            userManager.AddToRole(dispatcher.Id, Roles.Dispatcher.ToString());
            userManager.AddToRole(driver.Id, Roles.Driver.ToString());
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