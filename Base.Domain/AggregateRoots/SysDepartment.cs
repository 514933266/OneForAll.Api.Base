using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Base.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：部门组织
    /// </summary>
    public class SysDepartment : AggregateRoot<Guid>, ICreateTime, IParent<Guid>
    {
        public SysDepartment()
        {
            SysJobs = new HashSet<SysJob>();
        }

        /// <summary>
        /// 所属租户id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        public virtual SysTenant SysTenant { get; set; }

        /// <summary>
        /// 上级Id
        /// </summary>
        [Required]
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 排序号 由大到小
        /// </summary>
        [Required]
        public int SortNumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Remark { get; set; } = "";

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public virtual ICollection<SysJob> SysJobs { get; set; }

        /// <summary>
        /// 添加岗位
        /// </summary>
        /// <param name="entity">岗位</param>
        /// <returns></returns>
        public BaseErrType AddJob(SysJob entity)
        {
            if (SysJobs.Any(w => w.Name.Equals(entity.Name)))
            {
                return BaseErrType.DataExist;
            }
            else
            {
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }
                entity.SysDepartmentId = Id;
                SysJobs.Add(entity);
                return BaseErrType.Success;
            }
        }

        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="jobId">岗位id</param>
        /// <returns></returns>
        public void RemoveJob(Guid jobId)
        {
            var item = SysJobs.FirstOrDefault(w => w.Id == jobId);
            if (item != null)
            {
                SysJobs.Remove(item);
            }
        }

        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="jobIds">岗位id</param>
        /// <returns></returns>
        public void RemoveJob(IEnumerable<Guid> jobIds)
        {
            jobIds.ForEach(jobId =>
            {
                RemoveJob(jobId);
            });
        }
    }
}
