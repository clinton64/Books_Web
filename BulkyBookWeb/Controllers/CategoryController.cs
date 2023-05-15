using BulkyBook.DataAccess;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> categories = _context.Categories.ToList();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if(category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name","Custom Error");
            }
            if (ModelState.IsValid)
            {
                TempData["Success"] = "Category created successfully";
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Edit(int ? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var category = _context.Categories.Find(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Custom Error");
            }
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
                TempData["success"] = "Category Updated Succesfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var category = _context.Categories.Find(id); 
            if(category == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                TempData["success"] = "Category Deleted Succesfully";
                _context.Categories.Remove(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}
