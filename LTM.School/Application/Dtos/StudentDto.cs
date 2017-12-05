using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.Application.Dtos
{
    public class StudentDto
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [RequiredAttribute]
        [StringLength(30, ErrorMessage = "名字过长")]
        [DisplayName("学生姓名")]
        public string RealName { get; set; }
        /// <summary>
        /// 入学时间
        /// </summary>
        [DisplayName("注册时间")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] //ApplyFormatInEditMode 映射到model 格式类型是DataFormatString
        public DateTime EnrollmentDate { get; set; }

    }
}
