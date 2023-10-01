using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SchedualingSystem.Models.IdentityEntities; 
using Microsoft.EntityFrameworkCore;
namespace SchedualingSystem.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser >
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }

    }
}
