﻿using IdentityServer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using IdentityServer4.EntityFramework.Extensions;

namespace IdentityServer.Date
{
    //nuget:Microsoft.AspNetCore.Identity.EntityFrameworkCore
    //https://www.cnblogs.com/lonelyxmas/p/10597446.html
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public DbSet<ApplicationUseAuthorization> applicationUseAuthorizations { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //如果默认的字段不满足自己，可以修改这里
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(h => h.Id); //主键
                entity.Property(p => p.Id).ValueGeneratedOnAdd(); //主键自增
                //entity.Ignore(i => i.NormalizedEmail); //忽略该字段，即：不映射该字段到表中
            });

            builder.Entity<ApplicationUseAuthorization>(e => {
                e.ToTable("AspNetUserAuthorizations")
                .Property(_=>_.Id).ValueGeneratedOnAdd().HasDefaultValue(1);
                e.Property(_ => _.Created).HasDefaultValue(DateTime.Now);
                e.HasKey(_ => _.Id);
                
            });
        }
    }
    //http://www.it1352.com/684096.html
    public class aa : DbContext, IConfigurationDbContext
    {
        private readonly ConfigurationStoreOptions storeOptions;
        public aa(DbContextOptions<aa> options)
         : base(options)
        {
        }
        //IdentityConfigurationDbContext
        public aa(DbContextOptions<ConfigurationDbContext> options, ConfigurationStoreOptions storeOptions)
        : base(options)
        {
            this.storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<IdentityResource> IdentityResources { get; set; }
        public DbSet<ApiResource> ApiResources { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
            //throw new NotImplementedException();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiResource>(c => c.HasKey(a => a.Id));



            modelBuilder.ConfigureClientContext(storeOptions);
            modelBuilder.ConfigureResourcesContext(storeOptions);
            base.OnModelCreating(modelBuilder);
        }
    }

    public class test : DbContext, IPersistedGrantDbContext
    {
        private readonly OperationalStoreOptions storeOptions;

        public test(DbContextOptions<test> options)
         : base(options)
        {
        }
        //IdentityConfigurationDbContext
        public test(DbContextOptions<PersistedGrantDbContext> options, OperationalStoreOptions storeOptions)
        : base(options)
        {
            this.storeOptions = storeOptions ?? throw new ArgumentNullException(nameof(storeOptions));
        }

        public DbSet<PersistedGrant> PersistedGrants { get; set; }
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
            //throw new NotImplementedException();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigurePersistedGrantContext(storeOptions);
            base.OnModelCreating(modelBuilder);
        }
    }

    /// <summary>
    /// 扩展DeviceFlowCodes表
    /// </summary>
    public class ExtendDeviceFlowCodes //: DeviceFlowCodes
    {
        /// <summary>
        /// 备注
        /// </summary>
        public string AddRemark { get; set; }
    }
    public class RegisterPersistedGrant : PersistedGrantDbContext
    {
        ////new IdentityServer4.EntityFramework.Entities.PersistedGrant
        //public DbSet<PersistedGrant> persistedGrants { get; set; }
        //public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        //public DbSet<PersistedGrant> PersistedGrants { get; set; }

        public DbSet<ExtendDeviceFlowCodes> test1 { get; set; }

        public RegisterPersistedGrant(DbContextOptions<PersistedGrantDbContext> options, OperationalStoreOptions storeOptions) : base(options, storeOptions)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<DeviceFlowCodes>(e => { 
            //e.OwnsOne(e=>e.)
            //});
        }
    }
}