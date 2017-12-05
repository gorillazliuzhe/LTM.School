using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.ViewModels
{
    /// <summary>
    /// 老师分配课程信息
    /// </summary>
    public class AssignedCourseData
    {
        /// <summary>
        /// 课程ID
        /// </summary>
        public int CourseId { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 是否分配
        /// </summary>
        public bool Assigned { get; set; }
    }
}
