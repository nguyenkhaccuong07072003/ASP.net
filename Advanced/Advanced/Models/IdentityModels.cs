using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Advanced.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }
        public string Age { get; set; }
        public string Main_subject { get; set; }
        public string Address { get; set; }
        public DateTime? BirtDate { get; set; }
        public string Image { get; set; }
        public string ShortDesc { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Khoahoc> Khoahocs { get; set; }
        public DbSet<Lophoc> Lophocs { get; set; }
        public DbSet<RollOut> RollOuts { get; set; }
        public DbSet<ClassMember> ClassMembers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Lichhoc> Lichhocs { get; set; }
        public DbSet<Post_Post> Post_Posts { get; set; }
        public DbSet<Main_Comment> Main_Comments { get; set; }
        public DbSet<Sub_Comment> Sub_Comments { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<DatHang> DatHang { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}