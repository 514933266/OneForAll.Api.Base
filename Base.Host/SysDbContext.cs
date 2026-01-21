using System;
using Base.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Base.Host
{
    public partial class SysDbContext : DbContext
    {
        public SysDbContext(DbContextOptions<SysDbContext> options)
            : base(options)
        {

        }

        #region 菜单权限

        public virtual DbSet<SysMenu> SysMenu { get; set; }
        public virtual DbSet<SysPermission> SysPermission { get; set; }
        public virtual DbSet<SysMidTenantPermission> SysMidTenantPerm { get; set; }
        public virtual DbSet<SysMidUserPermission> SysMidUserPerm { get; set; }
        public virtual DbSet<SysMidRolePermission> SysMidRolePermission { get; set; }
        public virtual DbSet<SysMidRoleUser> SysMidRoleUser { get; set; }

        #endregion

        #region 系统用户

        public virtual DbSet<SysTenant> SysTenant { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<SysRole> SysRole { get; set; }
        public virtual DbSet<SysMidTenantUser> SysMidTenantUser { get; set; }
        #endregion

        #region 公共数据

        public virtual DbSet<SysArea> SysArea { get; set; }

        #endregion

        #region 系统通知

        public virtual DbSet<SysArticle> SysArticle { get; set; }
        public virtual DbSet<SysArticleRecord> SysArticleRecord { get; set; }
        public virtual DbSet<SysArticleType> SysArticleType { get; set; }

        #endregion

        #region 微信用户
        public virtual DbSet<SysWechatUser> SysWechatUser { get; set; }
        public virtual DbSet<SysWechatGzhSubscriber> SysWechatgzhSubscribeUser { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 菜单权限

            modelBuilder.Entity<SysMenu>(entity =>
            {
                entity.ToTable("sys_menu");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysPermission>(entity =>
            {
                entity.ToTable("sys_permission");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysMidTenantPermission>(entity =>
            {
                entity.ToTable("sys_mid_tenant_permission");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysMidRolePermission>(entity =>
            {
                entity.ToTable("sys_mid_role_permission");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysMidRoleUser>(entity =>
            {
                entity.ToTable("sys_mid_role_user");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysMidUserPermission>(entity =>
            {
                entity.ToTable("sys_mid_user_permission");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 系统用户

            modelBuilder.Entity<SysTenant>(entity =>
            {
                entity.ToTable("sys_tenant");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysRole>(entity =>
            {
                entity.ToTable("sys_role");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysUser>(entity =>
            {
                entity.ToTable("sys_user");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(b => b.UserName).IsUnique();
            });

            modelBuilder.Entity<SysMidTenantUser>(entity =>
            {
                entity.ToTable("sys_mid_tenant_user");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 公共数据

            modelBuilder.Entity<SysArea>(entity =>
            {
                entity.ToTable("sys_area");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 系统通知

            modelBuilder.Entity<SysArticleType>(entity =>
            {
                entity.ToTable("sys_article_type");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysArticle>(entity =>
            {
                entity.ToTable("sys_article");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysArticleRecord>(entity =>
            {
                entity.ToTable("sys_article_record");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

            #region 微信用户

            modelBuilder.Entity<SysWechatUser>(entity =>
            {
                entity.ToTable("sys_wechat_user");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SysWechatGzhSubscriber>(entity =>
            {
                entity.ToTable("sys_wechat_gzh_subscriber");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            #endregion

        }
    }
}
