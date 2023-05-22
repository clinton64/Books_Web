using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers; 

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(IUnitOfWork dbContext, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = dbContext;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index()
    {
        return View();
    }
    //Get
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            })
        };
           
        if (id == null || id == 0) 
        {
            //create
            return View(productVM);
        }
        else
        {
            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            return View(productVM);
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"Images\Product");
                var extension = Path.GetExtension(file.FileName);

                using(var fileStream = new FileStream(Path.Combine(uploads, file + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream); 
                }
                obj.Product.ImageURL = @"\Images\Product\" + fileName + extension;
            }
            _unitOfWork.Product.Add(obj.Product);
            _unitOfWork.Save();
            TempData["success"] = "Product Created Successfully";
            return RedirectToAction("Index");
        }
        return View(obj);

    }
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            TempData["success"] = "Category Deleted Succesfully";
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        return View(product);
    }


    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
        return Json(new {data = productList});  
    }
    #endregion
}
