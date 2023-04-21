using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Base.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：地区组
    /// </summary>
    public partial class SysAreaGroup : AggregateRoot<Guid>
    {
        public SysAreaGroup()
        {
            SysAreaGroupContacts = new HashSet<SysAreaGroupContact>();
            SysAreaGroupUserContacts = new HashSet<SysAreaGroupUserContact>();
        }

        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        public virtual SysTenant SysTenant { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Remark { get; set; } = "";

        public virtual ICollection<SysAreaGroupContact> SysAreaGroupContacts { get; set; }
        public virtual ICollection<SysAreaGroupUserContact> SysAreaGroupUserContacts { get; set; }

        /// <summary>
        /// 添加地区
        /// </summary>
        /// <param name="area">地区</param>
        public void AddArea(SysArea area)
        {
            var item = SysAreaGroupContacts.FirstOrDefault(w => w.AreaCode == area.Code);
            if (item == null)
            {
                SysAreaGroupContacts.Add(new SysAreaGroupContact()
                {
                    Id = Guid.NewGuid(),
                    SysAreaGroupId = Id,
                    AreaCode = area.Code
                });
            }
        }

        /// <summary>
        /// 添加地区
        /// </summary>
        /// <param name="areas"></param>
        public void AddArea(IEnumerable<SysArea> areas)
        {
            areas.ForEach(e =>
            {
                AddArea(e);
            });
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="users">用户</param>
        public void AddMember(IEnumerable<SysUser> users)
        {
            users.ForEach(e =>
            {
                var item = SysAreaGroupUserContacts.FirstOrDefault(w => w.SysUserId.Equals(e.Id));
                if (item == null)
                {
                    SysAreaGroupUserContacts.Add(new SysAreaGroupUserContact()
                    {
                        Id = Guid.NewGuid(),
                        SysAreaGroupId = Id,
                        SysUserId = e.Id
                    });
                }
            });
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="userId">用户id</param>
        public void RemoveMember(Guid userId)
        {
            var item = SysAreaGroupUserContacts.FirstOrDefault(w => w.SysUserId == userId);
            if (item != null)
            {
                SysAreaGroupUserContacts.Remove(item);
            }
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="userIds">用户id</param>
        public void RemoveMember(IEnumerable<Guid> userIds)
        {
            userIds.ForEach(userId =>
            {
                RemoveMember(userId);
            });
        }
    }
}
