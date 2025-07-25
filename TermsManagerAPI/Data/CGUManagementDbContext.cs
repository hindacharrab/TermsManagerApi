using CGUManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CGUManagementAPI.Data
{
    public class CGUManagementDbContext : DbContext
    {
        public CGUManagementDbContext(DbContextOptions<CGUManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CGU> CGUs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("User");
            /*  ****        */
            modelBuilder.Entity<User>()
                .HasOne(u => u.AcceptedCGU)
                .WithMany()
                .HasForeignKey(u => u.AcceptedCGUId)
                .OnDelete(DeleteBehavior.Restrict);
            /*  ****        */


            modelBuilder.Entity<CGU>()
                .Property(c => c.Version)
                .HasDefaultValue("1.0");

        }
    }
}
