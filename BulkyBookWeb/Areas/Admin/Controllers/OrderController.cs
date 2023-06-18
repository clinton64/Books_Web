using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModel;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM _orderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            _orderVM = new OrderVM
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(u => u.OrderId == orderId, includeProperties: "Product")
            };
            return View(_orderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == _orderVM.OrderHeader.Id); 
            orderHeaderFromDb.Name = _orderVM.OrderHeader.Name; 
            orderHeaderFromDb.PhoneNumber = _orderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = _orderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = _orderVM.OrderHeader.City;
            orderHeaderFromDb.PostalCode = _orderVM.OrderHeader.PostalCode;
            orderHeaderFromDb.State = _orderVM.OrderHeader.State;
            if(_orderVM.OrderHeader.Carrier != null)
            {
                orderHeaderFromDb.Carrier = _orderVM.OrderHeader.Carrier;
            }
            if(_orderVM.OrderHeader.PaymentIntentId != null)
            {
                orderHeaderFromDb.TrackingNumber = _orderVM.OrderHeader.TrackingNumber;
            }
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully.";
            return RedirectToAction("Details", "Order", new { orderId = orderHeaderFromDb.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeader.UpdateStatus(_orderVM.OrderHeader.Id, SD.StatusInProgress);
            _unitOfWork.Save();
            TempData["Success"] = "Order Status Updated Successfully";
            return RedirectToAction("Details", "Order", new {orderId =_orderVM.OrderHeader.Id});
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult ShipOrder()
		{
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == _orderVM.OrderHeader.Id, tracked: false);
            orderHeader.TrackingNumber = _orderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = _orderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;

            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
			TempData["Success"] = "Order Shipped Successfully";
			return RedirectToAction("Details", "Order", new { orderId = _orderVM.OrderHeader.Id });
		}


		#region API 
		[HttpGet]
        public IActionResult GetAll(string status) 
        {
            IEnumerable<OrderHeader> orderHeaders;
            if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");
            }
            
            switch (status)
            {
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.StatusInProgress);
                    break;
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.PaymentStatusPending);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.PaymentStatusApproved);
                    break;
                default:
                    break;
            }
                
            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
