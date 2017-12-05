using LTM.School.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.EntityFramework
{
    public class SchoolDbContext:DbContext
    {
        /// <summary>
        /// 通过DbContextOptions配置,使SchoolDbContext和数据库连通
        /// </summary>
        /// <param name="options"></param>
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options):base(options)
        {
        }

        // 新建得实体一定要注入到数据库链接上下文 
        public DbSet<Student> Students { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<CourseAssignment> CourseAssignments { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        // 视频说 必须用modelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // .ToTable("Student") 把表名字改成student 单数
            modelBuilder.Entity<Student>().ToTable("Student");
            // .Property(a=>a.CourseId).ValueGeneratedNever() 主键自己生成 不用系统生成
            // modelBuilder.Entity<Course>().ToTable("Course").Property(a=>a.CourseId).ValueGeneratedNever();
            // modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Course>().ToTable("Course").Property(a => a.CourseId).ValueGeneratedNever();
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            // .HasOne(a=>a.Course).WithMany(a=>a.Enrollments) Course课程和Enrollments登记表是一对多的关系
            // modelBuilder.Entity<Enrollment>().ToTable("Enrollment").HasOne(a=>a.Course).WithMany(a=>a.Enrollments);

            // modelBuilder.Entity<Enrollment>().ToTable("Enrollment").HasOne(a => a.Student).WithMany(a => a.Enrollments);

            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment").HasKey(a=>new { a.CourseId,a.InstructorId});
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");//.HasKey(a=>a.Id);

            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment").HasKey(a=>a.InstructorId);

        }
    }
}
