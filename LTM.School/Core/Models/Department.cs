using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.Core.Models
{
    /// <summary>
    /// 部门
    /// </summary>
    public class Department
    {
        public int Id { get; set; }

        [StringLength(50,MinimumLength =2)]
        [DisplayName("部门名称")]
        public string Name { get; set; }

        /// <summary>
        /// 部门预算
        /// </summary>
        [Column(TypeName ="money")] // 数据库中看见得是 money
        [DataType(DataType.Currency)]
        [DisplayName("部门预算")]
        public decimal Budget { get; set; }

        /// <summary>
        /// 开课时间
        /// </summary>
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [DataType(DataType.Date)]
        [Display(Name ="开课时间")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 教师ID主键
        /// </summary>
        [DisplayName("教师编号")]
        public int? InstructorId { get; set; }

        /// <summary>
        /// 办公室主任
        /// </summary>
        [DisplayName("办公室主任")]
        public Instructor Administrator { get; set; }

        /// <summary>
        /// 课程
        /// </summary>
        [DisplayName("分配课程")]
        public ICollection<Course> Courses { get; set; }

        [Timestamp] //字段发生改变 会记录时间那个在前那个在后
        public byte[] RowVersion { get; set; }
    }
}
