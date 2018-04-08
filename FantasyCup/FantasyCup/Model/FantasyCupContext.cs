using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FantasyCup.Model
{
    public partial class FantasyCupContext : DbContext
    {
        public virtual DbSet<League> League { get; set; }
        public virtual DbSet<LeagueUser> LeagueUser { get; set; }
        public virtual DbSet<User> User { get; set; }

        public FantasyCupContext() { }

        public FantasyCupContext(DbContextOptions<FantasyCupContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=HOODAHELL-PC\MSSQL16;Database=FantasyCup;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<League>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PotAmount).HasColumnType("money");
            });

            modelBuilder.Entity<LeagueUser>(entity =>
            {
                entity.HasKey(e => new { e.LeagueId, e.UserId });

                entity.HasOne(d => d.League)
                    .WithMany(p => p.LeagueUser)
                    .HasForeignKey(d => d.LeagueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeagueUser_League");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LeagueUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LeagueUser_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(600);

                entity.Property(e => e.PasswordHash).IsRequired();

                entity.Property(e => e.PasswordSalt).IsRequired();

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(200);
            });
        }
    }
}
