using Microsoft.AspNetCore.Mvc;
using Ticket15.DAL;
using Ticket15.Models;
using Microsoft.EntityFrameworkCore;

namespace Ticket15.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly string _uploadPath;

        public MemberController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
            _uploadPath = Path.Combine(_env.WebRootPath, "admin", "img");
        }

        public async Task<IActionResult> Index()
        {
            var members = await _dbContext.Members.AsNoTracking().ToListAsync();
            return View(members);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Member member)
        {
            if (member.PhotoFile == null)
            {
                ModelState.AddModelError("PhotoFile", "Şekil yuklemek mecburidir!");
            }

            if (!ModelState.IsValid) return View(member);

            if (!IsValidImage(member.PhotoFile, out string errorMessage))
            {
                ModelState.AddModelError("PhotoFile", errorMessage);
                return View(member);
            }

            member.ImgUrl = await SaveFileAsync(member.PhotoFile);

            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var member = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();

            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, Member member)
        {
            if (id == null || id != member.Id) return BadRequest();

            var existed = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (existed == null) return NotFound();

            if (!ModelState.IsValid)
            {
                member.ImgUrl = existed.ImgUrl;
                return View(member);
            }

            if (member.PhotoFile != null)
            {
                if (!IsValidImage(member.PhotoFile, out string errorMessage))
                {
                    ModelState.AddModelError("PhotoFile", errorMessage);
                    member.ImgUrl = existed.ImgUrl;
                    return View(member);
                }

                DeleteFile(existed.ImgUrl);
                existed.ImgUrl = await SaveFileAsync(member.PhotoFile);
            }

            existed.FullName = member.FullName;
            existed.Salary = member.Salary;
            existed.Point = member.Point;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var member = await _dbContext.Members.FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();

            DeleteFile(member.ImgUrl);
            _dbContext.Members.Remove(member);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool IsValidImage(IFormFile file, out string message)
        {
            if (file == null)
            {
                message = "Fayl secilmeyib.";
                return false;
            }
            if (!file.ContentType.Contains("image/"))
            {
                message = "Fayl formati sekil olmalidir";
                return false;
            }
            if (file.Length > 2 * 1024 * 1024)
            {
                message = "Sekil olcusu 2MB-dan cox ola bilmez";
                return false;
            }
            message = string.Empty;
            return true;
        }

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            if (!Directory.Exists(_uploadPath))
                Directory.CreateDirectory(_uploadPath);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }

        private void DeleteFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            string fullPath = Path.Combine(_uploadPath, fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}