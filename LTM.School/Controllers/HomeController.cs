using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LTM.School.EntityFramework;
using LTM.School.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Collections.Generic;

namespace LTM.School.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolDbContext _context;
        public HomeController(SchoolDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {

            ViewData["Message"] = "学生统计信息";


            var groups = new List<EnrollmentDateGroup>();

            var conn = _context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command=conn.CreateCommand())
                {
                    string sqlQuery = @"SELECT EnrollmentDate,COUNT(*) AS StudentCount FROM dbo.Person  WHERE Discriminator='Student' GROUP BY EnrollmentDate";
                    command.CommandText = sqlQuery;

                    DbDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new EnrollmentDateGroup() {
                                EnrollmentDate = reader.GetDateTime(0),
                                StudentCount=reader.GetInt32(1)
                            };
                            groups.Add(row);
                        }
                    }
                }
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
            return View(groups);

            //var entities = from student in _context.Students
            //           group student by student.EnrollmentDate into dateGroup
            //           select new EnrollmentDateGroup()
            //           {
            //               EnrollmentDate = dateGroup.Key,
            //               StudentCount = dateGroup.Count()
            //           };
            //var dtos = await entities.AsNoTracking().ToListAsync();
            //return View(dtos);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
