using BaseRMS.Entities;
using BaseRMS.Entities.Abstract;
using BaseRMS.Entities.AttatchmentClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BaseRMS.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options)
{

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.EnableSensitiveDataLogging();
#endif
    }


    public required DbSet<ActivityLog> ActivityLogs { get; set; }
    public required DbSet<ApplicationFile> ApplicationFiles { get; set; }
    public required DbSet<ApplicationRole> ApplicationRoles { get; set; }
    public required DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public required DbSet<Client> Clients { get; set; }
    public required DbSet<ClientAttachment> ClientAttachments { get; set; }
    public required DbSet<TrustedDevice> TrustedDevices { get; set; }
    public required DbSet<RefreshToken> RefreshTokens { get; set; }
    public required DbSet<Contract> Contracts { get; set; }
    public required DbSet<ContractAttachment> ContractAttachments { get; set; }
    public required DbSet<ContractTypes> ContractTypes { get; set; }
    public required DbSet<Employee> Employees { get; set; }
    public required DbSet<EmployeeAttachment> EmployeeAttachments { get; set; }
    public required DbSet<EmployeeContract> EmployeesContracts { get; set; }
    public required DbSet<Event> Events { get; set; }
    public required DbSet<EventAttachment> EventAttachments { get; set; }
    public required DbSet<EventCategory> EventCategories { get; set; }
    public required DbSet<Machine> Machines { get; set; }
    public required DbSet<MachineAttachment> MachineAttachments { get; set; }
    public required DbSet<MachineType> MachineTypes { get; set; }
    public required DbSet<PersonalIdentificationType> PersonalIdentificationTypes { get; set; }
    public required DbSet<Translation> Translations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the default schema for all entities in this context
        modelBuilder.HasDefaultSchema("rms");

        ConfigurePropertyConversions(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>().ToTable("asp_net_users");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("asp_net_user_tokens");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("asp_net_user_logins");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("asp_net_user_claims");
        modelBuilder.Entity<ApplicationRole>().ToTable("asp_net_roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("asp_net_user_roles");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("asp_net_role_claims");

        ConfigureAttachment<Client, ClientAttachment>(modelBuilder);


    }

    /// <summary>
	/// This method converts properties that are lists of enums to be stored as
	/// comma-separated lists of string values in the database.
	/// </summary>
	/// <param name="modelBuilder"></param>
	private void ConfigurePropertyConversions(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ApplicationRole>(entity =>
		{
			entity.Property(e => e.Permitions)
				.HasConversion(
					v => string.Join(',', v),
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
						  .ToList(),
				new ValueComparer<ICollection<string>>(
					(c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
					c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
					c => c.ToList()));
		});

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.Property(e => e.ActivityTypes)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                          .ToList(),
                new ValueComparer<ICollection<string>>(
                    (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
        });

    }

    private void ConfigureAttachment<TEntity, TAttachment>(ModelBuilder modelBuilder)
    where TEntity : class
    where TAttachment : Attachment<TEntity>
    {
        var entityName = typeof(TEntity).Name;
        var fkName = entityName + "Id";

        modelBuilder.Entity<TAttachment>(builder =>
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.EntityId)
                   .HasColumnName(fkName);

            builder.HasOne(a => a.Entity)
                   .WithMany()
                   .HasForeignKey(a => a.EntityId);

            builder.HasOne(a => a.File)
                   .WithMany()
                   .HasForeignKey(a => a.FileId);
        });
    }

}
