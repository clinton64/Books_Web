using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web.Helpers;

namespace BulkyBookWeb.Areas.Admin.Controllers; 

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CompanyController(IUnitOfWork dbContext)
    {
        _unitOfWork = dbContext;
    }
    public IActionResult Index()
    {
        return View();
    }
    //Get
    public IActionResult Upsert(int? id)
    {
        Company company = new();
        
        // create
        if (id == null || id == 0) 
        {
            
            return View(company);
        }
        // Update
        else
        {
            company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            return View(company);
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company obj)
    {
        if (ModelState.IsValid)
        {
            var message = "";
            if(obj.Id == 0)
            {
                _unitOfWork.Company.Add(obj);
                message = "Company Created Successfully";
            }
            else
            {
                _unitOfWork.Company.Update(obj);
                message = "Company Updated Successfully";
            } 
            _unitOfWork.Save();
            TempData["success"] = message;
            return RedirectToAction("Index");
        }
        return View(obj);

    }
 

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var companyList = _unitOfWork.Company.GetAll();
        return Json(new {data = companyList});  
    }


    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
        if (obj == null)
        {
            return Json(new {success = false, message = "Deletion unsuccessful"});
        }
        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true , message = "deletion succesfull"}) ;
    }
    #endregion
}
 