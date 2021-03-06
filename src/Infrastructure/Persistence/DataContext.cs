using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Persistence
{
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
     UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Quest> Quests { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<BoardViewer> BoardViewers { get; set; }
        public DbSet<BoardEditor> BoardEditors { get; set; }
        public DbSet<MessagePost> Messages { get; set; }
        
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<BoardViewer>()
                .HasKey(bv => new { bv.BoardId, bv.ViewerId });
            builder.Entity<BoardViewer>()
                .HasOne(bv => bv.Board)
                .WithMany(b => b.BoardViewers)
                .HasForeignKey(bv => bv.BoardId);
            builder.Entity<BoardViewer>()
                .HasOne(bv => bv.Viewer)
                .WithMany(v => v.ViewBoards)
                .HasForeignKey(bv => bv.ViewerId);

            builder.Entity<BoardEditor>()
                .HasKey(be => new { be.BoardId, be.EditorId });
            builder.Entity<BoardEditor>()
                .HasOne(be => be.Board)
                .WithMany(b => b.BoardEditors)
                .HasForeignKey(be => be.BoardId);
            builder.Entity<BoardEditor>()
                .HasOne(be => be.Editor)
                .WithMany(e => e.EditBoards)
                .HasForeignKey(be => be.EditorId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                        .Entries()
                        .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).DateModified = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).DateCreated = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=den1.mssql7.gear.host;Database=tasknote;User Id=tasknote;Password=Ou6Y9!4-0s7w;");
            }
        }
    }
}