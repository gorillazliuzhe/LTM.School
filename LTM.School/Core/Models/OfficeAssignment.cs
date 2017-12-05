using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.Core.Models
{
    /// <summary>
    /// 分配办公室
    /// </summary>
    public class OfficeAssignment
    {
        /// <summary>
        /// 教师Id
        /// </summary>
        [Key]
        public int InstructorId { get; set; }
        /// <summary>
        /// 教师导航属性
        /// </summary>
        public Instructor Instructor { get; set; }

        /// <summary>
        /// 办公室位置
        /// </summary>
        [StringLength(50)]
        [Display(Name ="办公室位置")]
        public string Location { get; set; }
    }
}
