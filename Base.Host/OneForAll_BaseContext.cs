using System;
using Base.Domain;
using Base.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Base.Host
{
    public partial class OneForAll_BaseContext : DbContext
    {
        private Guid _tenantId;

        public OneForAll_BaseContext(DbContextOptions<OneForAll_BaseContext> options)
            : base(options)
        {
            
        }

        public OneForAll_BaseContext(
            DbContextOptions<OneForAll_BaseContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantId = tenantProvider.GetTenantId();
        }

        public virtual DbSet<SysArea> SysArea { get; set; }
        public virtual DbSet<SysMenu> SysMenu { get; set; }
        public virtual DbSet<SysPermission> SysPermission { get; set; }
        public virtual DbSet<SysRole> SysRole { get; set; }
        public virtual DbSet<SysRolePermContact> SysRolePermContact { get; set; }
        public virtual DbSet<SysRoleUserContact> SysRoleUserContact { get; set; }
        public virtual DbSet<SysTenant> SysTenant { get; set; }
        public virtual DbSet<SysTenantPermContact> SysTenantPermContact { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }

        public virtual DbSet<SysUserPermContact> SysUserPermContact { get; set; }

        public virtual DbSet<SysArticleType> SysArticleType { get; set; }
        public virtual DbSet<SysArticle> SysArticle { get; set; }

        public virtual DbSet<SysArticleRecord> SysArticleRecord { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SysArea>(form =>
            {
                form.ToTable("Sys_Area");

                form.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysMenu>(form =>
            {
                form.ToTable("Sys_Menu");

                form.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysPermission>(form =>
            {
                form.ToTable("Sys_Permission");

                form.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysRole>(form =>
            {
                form.ToTable("Sys_Role");

                form.Property(e => e.Id).ValueGeneratedOnAdd();

                form.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<SysRolePermContact>(form =>
            {
                form.ToTable("Sys_RolePermContact");

                form.Property(e => e.Id).ValueGeneratedOnAdd();

            });

            modelBuilder.Entity<SysRoleUserContact>(form =>
            {
                form.ToTable("Sys_RoleUserContact");

                form.Property(e => e.Id).ValueGeneratedOnAdd();

            });

            modelBuilder.Entity<SysTenant>(form =>
            {
                form.ToTable("Sys_Tenant");

                form.Property(e => e.Id).ValueGeneratedOnAdd();

                form.HasQueryFilter(e => (e.Id == _tenantId || e.ParentId == _tenantId));
            });

            modelBuilder.Entity<SysTenantPermContact>(form =>
            {
                form.ToTable("Sys_TenantPermContact");

                form.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysUser>(form =>
            {
                form.ToTable("Sys_User");

                form.Property(e => e.Id).ValueGeneratedOnAdd();

                form.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<SysUserPermContact>(form =>
            {
                form.ToTable("Sys_UserPermContact");

                form.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysArticleType>(form =>
            {
                form.ToTable("Sys_ArticleType");

                form.Property(e => e.Id).ValueGeneratedOnAdd();

                form.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<SysArticle>(form =>
            {
                form.ToTable("Sys_Article");

                form.Property(e => e.Id).ValueGeneratedOnAdd();

                form.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<SysArticleRecord>(form =>
            {
                form.ToTable("Sys_ArticleRecord");

                form.Property(e => e.Id).ValueGeneratedOnAdd();

            });
        }
    }
}
