using LTM.School.Application.enumsType;
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
    /// 课程表
    /// </summary>
    public class Course
    {
        // 主键自己写,不用自动生成
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "课程编号")]
        public int CourseId { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        [StringLength(50, MinimumLength = 2)]
        [DisplayName("课程名称")]
        public string Title { get; set; }
        /// <summary>
        /// 学分
        /// </summary>
        [Range(0,5)] // 范围
        [DisplayName("学分")]
        public int Credits { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        [DisplayName("课程成绩")]
        public CourseGrade Grade { get; set; }
      
        /// <summary>
        /// 部门ID
        /// </summary>
        public int DepartmentId { get; set; }
        [DisplayName("部门信息")]
        public Department Department { get; set; }

        /// <summary>
        /// 课程分配到属性
        /// </summary>

        public ICollection<CourseAssignment> CourseAssignments { get; set; }

        /// <summary>
        /// 课程和学生一对多
        /// </summary>
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
