using LTM.School.Application.enumsType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.Core.Models
{
    /// <summary>
    /// 登记表(学生和课程关联关系表)
    /// </summary>
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public CourseGrade? Grade { get; set; }
        public  Student Student { get; set; }
        public Course Course { get; set; }
    }
}
