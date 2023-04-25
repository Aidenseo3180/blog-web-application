using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Web.Data
{
    //inherit from identityDbContext
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //***seed the roles (User, Admin, SuperAdmin)

            var AdminRoleId = "1d637f9d-f57b-45a3-b539-78e25da30eaf"; //unique Guid id created using C# interactive for admin
            var SuperAdminRoleId = "7bcd703b-d9f6-4b7f-99e0-df9cc503e346";  //for superadmin
            var UserRoleId = "0dd1a630-adf7-4a9f-a08d-8ecef762b120";    //for user
            
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name="Admin",
                    NormalizedName="Admin",
                    Id=AdminRoleId,
                    ConcurrencyStamp=AdminRoleId
                },
                new IdentityRole
                {
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id=SuperAdminRoleId,
                    ConcurrencyStamp=SuperAdminRoleId
                },
                new IdentityRole
                {
                    Name="User",
                    NormalizedName="User",
                    Id=UserRoleId,
                    ConcurrencyStamp=UserRoleId
                }
            };  //create object for roles

            builder.Entity<IdentityRole>().HasData(roles);   //when this runs, inserted roles to database

            //***seed SuperAdminUser

            var superAdminId = "c39e0482-946b-4644-850b-f31fbe812246";
            //identity user for superadmin user (create object)
            var superAdmin = new IdentityUser {
                UserName="SuperAdmin",
                Email="superadmin@blog.com",
                NormalizedEmail="superadmin@blog.com".ToUpper(),
                NormalizedUserName="superadmin@blog.com".ToUpper(),
                Id= superAdminId
            };

            //create password for superadmin
            superAdmin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(superAdmin, "Superadmin@123");

            //insert and seed to database
            builder.Entity<IdentityUser>().HasData(superAdmin);

            //***Add all the roles to SuperAdminUser (b/c superadmin can login as both user, admin, superadmin)

            //mapping roles with SuperAdmin
            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                //mapping roles with users
                new IdentityUserRole<string>
                {
                    RoleId=SuperAdminRoleId,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId=AdminRoleId,
                    UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId=UserRoleId,
                    UserId=superAdminId
                }
            };

            //then insert/seed to database
            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
