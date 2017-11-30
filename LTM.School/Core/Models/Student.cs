using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.Core.Models
{
    /// <summary>
    /// 学生表
    /// </summary>
    public class Student
    {
        /// <summary>
        /// 学号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 入学时间
        /// </summary>
        public DateTime EnrollmentDate { get; set; }
        /// <summary>
        /// 单个学生和课程一对多的关系
        /// </summary>
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
