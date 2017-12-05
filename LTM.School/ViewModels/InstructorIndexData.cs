using LTM.School.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.ViewModels
{
    public class InstructorIndexData
    {
        public List<Instructor> Instructors { get; set; }

        public List<Course> Courses { get; set; }

        public List<Enrollment> Enrollments { get; set; }
    }
}
