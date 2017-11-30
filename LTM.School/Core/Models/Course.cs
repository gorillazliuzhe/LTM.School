using LTM.School.Application.enumsType;
using System;
using System.Collections.Generic;
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
        public int CourseId { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 学分
        /// </summary>
        public int Credits { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public CourseGrade Grade {get; set; }
        /// <summary>
        /// 课程和学生一对多
        /// </summary>
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
