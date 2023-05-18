using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork dbContext)
        {
            _unitOfWork = dbContext;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> coverTypes = _unitOfWork.CoverType.GetAll();
            return View(coverTypes);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            //if (coverType.Name == coverType.Id.ToString())
            //{
            //    ModelState.AddModelError("name", "Custom Error");
            //}
            if (ModelState.IsValid)
            {
                TempData["Success"] = "CoverType created successfully";
                _unitOfWork.CoverType.Add(coverType);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(coverType);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType coverType)
        {
            //if (coverType.Name == coverType.Id.ToString())
            //{
            //    ModelState.AddModelError("name", "Custom Error");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(coverType);
                _unitOfWork.Save();
                TempData["success"] = "CoverType Updated Succesfully";
                return RedirectToAction("Index");
            }
            return View(coverType);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                TempData["success"] = "Category Deleted Succesfully";
                _unitOfWork.CoverType.Remove(coverType);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(coverType);
        }
    }
}
