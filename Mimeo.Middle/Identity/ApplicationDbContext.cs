using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mimeo.Middle.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Instance> Instances { get; set; }
        
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public ApplicationDbContext() : base(new DbContextOptions<ApplicationDbContext>())
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // This exists for development purposes only
                //
                optionsBuilder.UseSqlServer("Server=localhost;Database=MimeoBase;Trusted_Connection=True;");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.Id).HasMaxLength(128);
            });

            modelBuilder.Entity<IdentityRole>(b =>
            {
                b.Property(u => u.Id).HasMaxLength(128);
            });

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.HasOne<Instance>(x => x.Instance)
                    .WithMany(x => x.ApplicationUsers)
                    .HasForeignKey(x => x.InstanceId)
                    .IsRequired(false);

                b.HasNoDiscriminator();
            });
        }
    }
}

