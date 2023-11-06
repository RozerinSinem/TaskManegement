using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManegementSystem.Areas.Identity.Data;

namespace TaskManegementSystem.Data;

public class TaskManegementDbContext : IdentityDbContext<TaskManegementSystemUser>
{
    public TaskManegementDbContext(DbContextOptions<TaskManegementDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserIdentityConfiguration());
    }
    public class ApplicationUserIdentityConfiguration: IEntityTypeConfiguration<TaskManegementSystemUser>
    {
        

        public void Configure(EntityTypeBuilder<TaskManegementSystemUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(250);
            builder.Property(u => u.LastName).HasMaxLength(250);
        }
    }
}
