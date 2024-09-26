using System;
using Base.Domain;
using Base.Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using OneForAll.Core.DDD;

namespace Base.Host
{
    public partial class BaseDbContext : DbContext
    {
        private Guid _tenantId;

        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options)
        {

        }

        public BaseDbContext(
            DbContextOptions<BaseDbContext> options,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _tenantId = tenantProvider.GetTenantId();
        }

        #region 菜单权限

        public virtual DbSet<SysMenu> SysMenu { get; set; }
        public virtual DbSet<SysPermission> SysPermission { get; set; }
        public virtual DbSet<SysTenantPermContact> SysTenantPermContact { get; set; }
        public virtual DbSet<SysUserPermContact> SysUserPermContact { get; set; }
        public virtual DbSet<SysRolePermContact> SysRolePermContact { get; set; }
        public virtual DbSet<SysRoleUserContact> SysRoleUserContact { get; set; }

        #endregion

        #region 系统用户

        public virtual DbSet<SysTenant> SysTenant { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<SysTenantUserContact> SysTenantUserContact { get; set; }
        public virtual DbSet<SysRole> SysRole { get; set; }
        
        #endregion

        #region 公共数据

        public virtual DbSet<SysArea> SysArea { get; set; }

        #endregion

        #region 系统通知

        public virtual DbSet<SysArticle> SysArticle { get; set; }
        public virtual DbSet<SysArticleRecord> SysArticleRecord { get; set; }
        public virtual DbSet<SysArticleType> SysArticleType { get; set; }
        public virtual DbSet<UmsMessage> UmsMessage { get; set; }

        #endregion

        #region 微信用户

        public virtual DbSet<SysWechatUser> SysWechatUser { get; set; }
        public virtual DbSet<SysWxgzhSubscribeUser> SysWxgzhSubscribeUser { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 菜单权限

            modelBuilder.Entity<SysMenu>(entity =>
            {
                entity.ToTable("Sys_Menu");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysPermission>(entity =>
            {
                entity.ToTable("Sys_Permission");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysTenantPermContact>(entity =>
            {
                entity.ToTable("Sys_TenantPermContact");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysRolePermContact>(entity =>
            {
                entity.ToTable("Sys_RolePermContact");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysRoleUserContact>(entity =>
            {
                entity.ToTable("Sys_RoleUserContact");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysUserPermContact>(entity =>
            {
                entity.ToTable("Sys_UserPermContact");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 系统用户

            modelBuilder.Entity<SysTenant>(entity =>
            {
                entity.ToTable("Sys_Tenant");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasQueryFilter(e => (e.Id == _tenantId || e.ParentId == _tenantId));
            });

            modelBuilder.Entity<SysRole>(entity =>
            {
                entity.ToTable("Sys_Role");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.ToTable("Sys_User");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(b => b.UserName).IsUnique();
                entity.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<SysTenantUserContact>(entity =>
            {
                entity.ToTable("Sys_TenantUserContact");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 公共数据

            modelBuilder.Entity<SysArea>(entity =>
            {
                entity.ToTable("Sys_Area");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 系统通知

            modelBuilder.Entity<SysArticleType>(entity =>
            {
                entity.ToTable("Sys_ArticleType");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<SysArticle>(entity =>
            {
                entity.ToTable("Sys_Article");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<SysArticleRecord>(entity =>
            {
                entity.ToTable("Sys_ArticleRecord");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 微信用户

            modelBuilder.Entity<SysWechatUser>(entity =>
            {
                entity.ToTable("Sys_WechatUser");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysWxgzhSubscribeUser>(entity =>
            {
                entity.ToTable("Sys_WxgzhSubscribeUser");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion
        }
    }
}
