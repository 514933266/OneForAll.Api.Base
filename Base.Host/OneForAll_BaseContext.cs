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

            modelBuilder.Entity<SysTenantPermContact>(form =>
            {
                form.ToTable("Sys_TenantPermContact");
                form.Property(e => e.Id).ValueGeneratedOnAdd();
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

            modelBuilder.Entity<SysUserPermContact>(form =>
            {
                form.ToTable("Sys_UserPermContact");
                form.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 系统用户

            modelBuilder.Entity<SysTenant>(form =>
            {
                form.ToTable("Sys_Tenant");
                form.Property(e => e.Id).ValueGeneratedOnAdd();
                form.HasQueryFilter(e => (e.Id == _tenantId || e.ParentId == _tenantId));
            });

            modelBuilder.Entity<SysRole>(form =>
            {
                form.ToTable("Sys_Role");
                form.Property(e => e.Id).ValueGeneratedOnAdd();
                form.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<SysUser>(form =>
            {
                form.ToTable("Sys_User");
                form.Property(e => e.Id).ValueGeneratedOnAdd();
                form.HasQueryFilter(e => e.SysTenantId == _tenantId);
            });

            modelBuilder.Entity<SysTenantUserContact>(form =>
            {
                form.ToTable("Sys_TenantUserContact");
                form.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 公共数据

            modelBuilder.Entity<SysArea>(form =>
            {
                form.ToTable("Sys_Area");
                form.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 系统通知

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

            #endregion

            #region 微信用户

            modelBuilder.Entity<SysWechatUser>(form =>
            {
                form.ToTable("Sys_WechatUser");
                form.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysWxgzhSubscribeUser>(form =>
            {
                form.ToTable("Sys_WxgzhSubscribeUser");
                form.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion
        }
    }
}
