using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SCR.API.Models.Domain;

namespace SCR.API.Data
{
    public class SCRAuthDbContext : IdentityDbContext
    {
        public SCRAuthDbContext(DbContextOptions<SCRAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "48f2f082-03b0-467f-8efc-9c364a952a6c";
            var writerRoleId = "7aba91b0-80f6-463c-a603-fca8b9dcd939";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "reader".ToUpper()
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper()
                }
                    

            };
            builder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
