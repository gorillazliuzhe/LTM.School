using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.Core.Models
{
    /// <summary>
    /// 学生表
    /// </summary>
    public class Student: Person
    {
        /// <summary>
        /// 学号
        /// </summary>
       // public int Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        //[Required]
        //[StringLength(30,ErrorMessage ="名字过长")]
        //[DisplayName("学生姓名")]
        //public string RealName { get; set; }
        /// <summary>
        /// 入学时间
        /// </summary>
        [DisplayName("注册时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)] //ApplyFormatInEditMode 映射到model 格式类型是DataFormatString
        public DateTime EnrollmentDate { get; set; }
        /// <summary>
        /// 单个学生和课程一对多的关系
        /// </summary>
        [DisplayName("登记信息")]
        public ICollection<Enrollment> Enrollments { get; set; }

        [MaxLength(200)]
        public string Secret { get; set; }
    }
}
