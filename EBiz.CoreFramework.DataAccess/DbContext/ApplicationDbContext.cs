using EBiz.CoreFramework.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EBiz.CoreFramework.DataAccess.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<IdentityRole> AspNetRoles { get; set; }
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<IdentityUserRole<string>> AspNetUserRoles { get; set; }
        public DbSet<Pages> pages { get; set; }
        public DbSet<Notification> notification { get; set; }
        public DbSet<Menu> menu { get; set; }       
        public DbSet<Roles> roles { get; set; }       
        public DbSet<RolePermission> role_permissions { get; set; }       
        public DbSet<ExceptionLog> exception_log { get; set; }       
        public DbSet<Company> company { get; set; }       

        public DbSet<SiteEmail> site_email { get; set; }       
        public DbSet<SiteNotification> site_notification { get; set; }       
        public DbSet<SiteAWSProperty> site_aws_property { get; set; }       
    }
}
