using LTM.School.Application.Dtos;
using LTM.School.Core.Models;
using LTM.School.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolDbContext _context;

        public StudentsController(SchoolDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var student = await _context.Students
            //    .SingleOrDefaultAsync(m => m.Id == id);


            var student = await _context.Students.Include(a => a.Enrollments).ThenInclude(e => e.Course).AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] // 跨站请求验证 很重要
        public async Task<IActionResult> Create(StudentDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = new Student
                    {
                        RealName = dto.RealName,
                        EnrollmentDate = dto.EnrollmentDate
                    };
                    _context.Add(entity);
                    await _context.SaveChangesAsync();
                    //return View(student);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex) // 这种异常DbUpdateException 是指数据有问题
            {
                ModelState.AddModelError("", "无法进行数据保存,请检查数据是否异常");
            }

            return View(dto);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] //ActionName("Edit" 即使方法名是EditPOST但是实际路由还是edit
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RealName,EnrollmentDate")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }
            // 改进版本
            var entity = await _context.Students.SingleOrDefaultAsync(a => a.Id == id);
            if (ModelState.IsValid)
            {
                if (await TryUpdateModelAsync<Student>(entity, "", s => s.RealName, s => s.EnrollmentDate))
                {
                    try
                    {

                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError("", "无法进行数据保存,请检查数据是否异常");
                        throw;
                    }
                }
            }
           
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(student);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!StudentExists(student.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id,bool? saveChangesError=false)
        {
            if (id == null)
            {
                return NotFound();
            }
            // AsNoTracking() 会提高效率
            var student = await _context.Students.AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            // 修改
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.SaveError = "删除失败,联系系统管理员";
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.AsNoTracking().SingleOrDefaultAsync(m => m.Id == id);
            if (student==null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception)
            {
                return RedirectToAction(nameof(Delete), new { id=id, saveChangesError=true });
            }
                   
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
