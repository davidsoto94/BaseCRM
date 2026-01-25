using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BaseCRM.DbContexts;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the default schema for all entities in this context
        modelBuilder.HasDefaultSchema("crm");

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<IdentityUser>().ToTable("asp_net_users");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("asp_net_user_tokens");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("asp_net_user_logins");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("asp_net_user_claims");
        modelBuilder.Entity<IdentityRole>().ToTable("asp_net_roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("asp_net_user_roles");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("asp_net_role_claims");
    }

}
