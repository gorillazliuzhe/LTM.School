using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.Core.Models
{
    /// <summary>
    /// 人
    /// </summary>
    public class Person
    {
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [StringLength(30, ErrorMessage = "名字过长")]
        [DisplayName("姓名")]
        public string RealName { get; set; }


    }
}
