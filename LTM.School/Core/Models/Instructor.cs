using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.Core.Models
{
    /// <summary>
    /// 教师
    /// </summary>
    public class Instructor:Person
    {
        //public int Id { get; set; }
        //[DisplayName("教师名字")]
        //public string RealName { get; set; }

        [DisplayName("入职时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }

        /// <summary>
        /// 老师表和课程表是一对多得关系,一个老师可以教多个课程
        /// </summary>
        public ICollection<CourseAssignment> CourseAssignments { get; set; }

        /// <summary>
        /// 老师表和办公室位置表 一对一
        /// </summary>
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}
