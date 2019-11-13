using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Info.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Info.Date
{
    public class InfoDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<PersistedGrant> persistedGrants { get; set; }
        public InfoDbContext(DbContextOptions<InfoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users").HasKey(_=>_.Id);
                entity.Property(_ => _.Id).ValueGeneratedOnAdd();
                entity.Property(_ => _.EmailConfirmed).HasDefaultValue(0);
                entity.Property(_ => _.PhoneNumberConfirmed).HasDefaultValue(0);
                entity.Property(_ => _.Created).HasDefaultValue(DateTime.Now);
                entity.Property(_ => _.UserName).HasColumnType("nvarchar(50)");
                entity.Property(_ => _.Address).HasColumnType("nvarchar(100)");
                entity.Property(_ => _.Email).HasColumnType("nvarchar(50)");
                entity.Property(_ => _.PhoneNumber).HasColumnType("nvarchar(20)");
                entity.Property(_ => _.PassWord).HasColumnType("nvarchar(100)");
                entity.Property(_ => _.Source).HasColumnType("nvarchar(10)").HasDefaultValue("local"); //默认是本地
            });

            modelBuilder.Entity<PersistedGrant>(entity =>
            {
                entity.ToTable("PersistedGrants").HasKey(_ => _.Id);
                entity.Property(_ => _.Id).ValueGeneratedOnAdd();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
