using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FestivalFusion.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Role Ids
            var readerRoleId = "201";
            var writerRoleId = "202";
            var editorRoleId = "203";
            var moderatorRoleId = "204";

            // Adding the Reader, Writer, Editor and Moderator Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                },
                new IdentityRole()
                {
                    Id = editorRoleId,
                    Name = "Editor",
                    NormalizedName = "Editor".ToUpper(),
                    ConcurrencyStamp = editorRoleId
                },
                new IdentityRole()
                {
                    Id = moderatorRoleId,
                    Name = "Moderator",
                    NormalizedName = "Moderator".ToUpper(),
                    ConcurrencyStamp = moderatorRoleId
                }
            };

            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            // Seed role permissions as role claims.
            // Using claim type "permission" and values: "View", "Add", "Edit", "Delete"
            // Based on provided matrix:
            // Reader  -> View
            // Writer  -> View, Add
            // Editor  -> View, Add, Edit, Delete
            // Moderator -> View, Edit

            var roleClaims = new List<IdentityRoleClaim<string>>
            {
                // Reader (View)
                new IdentityRoleClaim<string> { Id = 1, RoleId = readerRoleId, ClaimType = "permission", ClaimValue = "View" },

                // Writer (View, Add)
                new IdentityRoleClaim<string> { Id = 2, RoleId = writerRoleId, ClaimType = "permission", ClaimValue = "View" },
                new IdentityRoleClaim<string> { Id = 3, RoleId = writerRoleId, ClaimType = "permission", ClaimValue = "Add" },

                // Editor (View, Add, Edit, Delete)
                new IdentityRoleClaim<string> { Id = 4, RoleId = editorRoleId, ClaimType = "permission", ClaimValue = "View" },
                new IdentityRoleClaim<string> { Id = 5, RoleId = editorRoleId, ClaimType = "permission", ClaimValue = "Add" },
                new IdentityRoleClaim<string> { Id = 6, RoleId = editorRoleId, ClaimType = "permission", ClaimValue = "Edit" },
                new IdentityRoleClaim<string> { Id = 7, RoleId = editorRoleId, ClaimType = "permission", ClaimValue = "Delete" },

                // Moderator (View, Edit)
                new IdentityRoleClaim<string> { Id = 8, RoleId = moderatorRoleId, ClaimType = "permission", ClaimValue = "View" },
                new IdentityRoleClaim<string> { Id = 9, RoleId = moderatorRoleId, ClaimType = "permission", ClaimValue = "Edit" }
            };

            builder.Entity<IdentityRoleClaim<string>>().HasData(roleClaims);

            // Create an Admin User 
            var adminUserId = "113";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@mail.com",
                Email = "admin@mail.com",
                NormalizedEmail = "admin@mail.com".ToUpper(),
                NormalizedUserName = "admin@mail.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

            builder.Entity<IdentityUser>().HasData(admin);

            // Give Roles To Admin
            // Assigning Admin to Editor role so the admin has full View/Add/Edit/Delete permissions.
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = editorRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }
    }
}
