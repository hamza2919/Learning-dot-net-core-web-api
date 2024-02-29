using Crud_Operation_Using_EF.General;
using Crud_Operation_Using_EF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crud_Operation_Using_EF.DAL
{
    public class EmployeeDBContext : IdentityDbContext<ApplicationUser>
    {
        public EmployeeDBContext(DbContextOptions<EmployeeDBContext> options)
            : base(options)
        {

        }

        public DbSet<Employees>? Employees { get; set; }
        public DbSet<AreaColony>? AreaColony { get; set; }
        public DbSet<Company>? Company { get; set; }
         
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRole(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            //builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }

        private void SeedRole(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(

                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "Manager", ConcurrencyStamp = "2", NormalizedName = "Manager" },
                new IdentityRole() { Name = "HR", ConcurrencyStamp = "3", NormalizedName = "HR" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "4", NormalizedName = "User" }
                );
        }
    }
}

//public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
//{
//    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
//    {
//        builder.Property(x => x.FirstName).HasMaxLength(255);
//        builder.Property(x => x.LastName).HasMaxLength(255);
//    }
//}