using BlazorDocManagerDotNet8.Server.Dto.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorDocManagerDotNet8.Server.AppDbContext
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public virtual DbSet<EventLogType> EventLogTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>()
            .Property(x => x.RowVersion)
                .IsRowVersion();//جلوگیری از همزمانی ویرایش

            builder.Entity<EventLogType>()
                .Property(x => x.RowVersion)
                .IsConcurrencyToken();

            base.OnModelCreating(builder);
        }
    }
}
