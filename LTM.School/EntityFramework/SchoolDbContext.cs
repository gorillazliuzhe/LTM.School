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

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Course { get; set; }

        public DbSet<Enrollment> Enrollments { get; set; }

        // 视频说 必须用modelBuilder
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 把表名字改成student 单数
            modelBuilder.Entity<Student>().ToTable("Student");
            // .Property(a=>a.CourseId).ValueGeneratedNever() 主键自己生成 不用系统生成
            // modelBuilder.Entity<Course>().ToTable("Course").Property(a=>a.CourseId).ValueGeneratedNever();
            // modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Course>().ToTable("Course").Property(a => a.CourseId).ValueGeneratedNever();
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            // .HasOne(a=>a.Course).WithMany(a=>a.Enrollments) Course课程和Enrollments登记表是一对多的关系
            // modelBuilder.Entity<Enrollment>().ToTable("Enrollment").HasOne(a=>a.Course).WithMany(a=>a.Enrollments);

            // modelBuilder.Entity<Enrollment>().ToTable("Enrollment").HasOne(a => a.Student).WithMany(a => a.Enrollments);
        }
    }
}
