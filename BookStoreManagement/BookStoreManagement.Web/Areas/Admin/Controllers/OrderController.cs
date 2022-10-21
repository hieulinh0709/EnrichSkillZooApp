using BookStoreManagement.Core.Constants;
using BookStoreManagement.DataAccess.Repository.IRepository;
using BookStoreManagement.Models;
using BookStoreManagement.Models.ViewModels;
using BookStoreManagement.Utility;
using BookStoreManagement.Web.Areas.Admin.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Data;
using System.Reflection;
using System.Security.Claims;

namespace BookStoreManagement.Web.Areas.Admin.Controllers
{
    [Area(ROLES.Admin)]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OrderService _orderService;
        public SessionService _sessionService;
        public SessionLineItemOptions _sessionLineItem;
        [BindProperty] // Binding dữ liệu đầu vào cho thuộc tính
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork, OrderService orderService = null)
        {
            _unitOfWork = unitOfWork;
            _orderService = orderService ?? new OrderService();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(u => u.Id == orderId, includeProperties: Entity.ApplicationUser),
                OrderDetail = _unitOfWork.OrderDetailRepo.GetAll(u => u.OrderId == orderId, includeProperties: Entity.Product),
            };

            return View(OrderVM);
        }

        /// <summary>
        /// Thực hiện thanh toán đơn hàng
        /// </summary>
        /// <returns></returns>
        [ActionName(ActionNameConsts.Details)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details_PAY_NOW()
        {
            OrderVM.OrderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, includeProperties: typeof(ApplicationUser).GetTypeInfo().Name);
            OrderVM.OrderDetail = _unitOfWork.OrderDetailRepo.GetAll(u => u.OrderId == OrderVM.OrderHeader.Id, includeProperties: Entity.Product);

            //stripe settings 
            var domain = "https://localhost:44300/";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = MODE.Payment,
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderHeaderid={OrderVM.OrderHeader.Id}",
                CancelUrl = domain + $"admin/order/details?orderId={OrderVM.OrderHeader.Id}",
            };

            foreach (var item in OrderVM.OrderDetail)
            {
                _sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),//20.00 -> 2000
                        Currency = Currency.USD,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        },

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(_sessionLineItem);

            }

            _sessionService = new SessionService();

            Session session = _sessionService.Create(options);
            _unitOfWork.OrderHeaderRepo.UpdateStripePaymentID(OrderVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(Response.StatusCode);
        }

        /// <summary>
        /// Xác nhận thanh toán đã thanh toán thì update trạng thái approved
        /// </summary>
        /// <param name="orderHeaderid"></param>
        /// <returns></returns>
        public IActionResult PaymentConfirmation(int orderHeaderid)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(u => u.Id == orderHeaderid);
            if (orderHeader.PaymentStatus == StatusData.PaymentStatusDelayedPayment)
            {
                _sessionService = new SessionService();
                Session session = _sessionService.Get(orderHeader.SessionId);
                //check the stripe status
                if (session.PaymentStatus.ToLower() == PaymentStatus.Paid)
                {
                    _unitOfWork.OrderHeaderRepo.UpdateStatus(orderHeaderid, orderHeader.OrderStatus, StatusData.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            return View(ViewNameConsts.PaymentConfirmation, orderHeaderid);
        }

        /// <summary>
        /// Cập nhật thông tin chi tiết cho đơn hàng
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = StatusData.Role_Admin + "," + StatusData.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetail()
        {
            var orderHEaderFromDb = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, tracked: false);
            orderHEaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHEaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHEaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHEaderFromDb.City = OrderVM.OrderHeader.City;
            orderHEaderFromDb.State = OrderVM.OrderHeader.State;
            orderHEaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            if (OrderVM.OrderHeader.Carrier != null)
                orderHEaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;

            if (OrderVM.OrderHeader.TrackingNumber != null)
                orderHEaderFromDb.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;

            _unitOfWork.OrderHeaderRepo.Update(orderHEaderFromDb);
            _unitOfWork.Save();
            TempData[STATUS.Success] = MSG.MsgCode1;
            return RedirectToAction(ActionNameConsts.Details, ControllerConsts.Order, new { orderId = orderHEaderFromDb.Id });
        }

        /// <summary>
        /// Đang xử lý, lấy hàng trong kho, gói hàng
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Roles = StatusData.Role_Admin + "," + StatusData.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcessing()
        {
            _unitOfWork.OrderHeaderRepo.UpdateStatus(OrderVM.OrderHeader.Id, StatusData.StatusInProcess);
            _unitOfWork.Save();
            TempData[STATUS.Success] = MSG.MsgCode2;
            return RedirectToAction(ActionNameConsts.Details, ControllerConsts.Order, new { orderId = OrderVM.OrderHeader.Id });
        }

        /// <summary>
        /// Giao cho dịch vụ vận chuyển
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = StatusData.Role_Admin + "," + StatusData.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult ShipOrder()
        {
            var orderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, tracked: false);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = StatusData.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;

            if (orderHeader.PaymentStatus == StatusData.PaymentStatusDelayedPayment)
                orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);

            _unitOfWork.OrderHeaderRepo.Update(orderHeader);
            _unitOfWork.Save();
            TempData[STATUS.Success] = MSG.MsgCode3;
            return RedirectToAction(ActionNameConsts.Details, ControllerConsts.Order, new { orderId = OrderVM.OrderHeader.Id });
        }

        /// <summary>
        /// Hủy đơn hàng
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = StatusData.Role_Admin + "," + StatusData.Role_Employee)]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderHeader = _unitOfWork.OrderHeaderRepo.GetFirstOrDefault(u => u.Id == OrderVM.OrderHeader.Id, tracked: false);

            // nếu đã thanh toán thì tạo request hoàn tiền và update status thành cancel 
            if (orderHeader.PaymentStatus == StatusData.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeaderRepo.UpdateStatus(orderHeader.Id, StatusData.StatusCancelled, StatusData.StatusRefunded);
            }
            else
                _unitOfWork.OrderHeaderRepo.UpdateStatus(orderHeader.Id, StatusData.StatusCancelled, StatusData.StatusCancelled);

            _unitOfWork.Save();

            /* 
            * Tương tự ViewData và ViewBag, TempData cũng dùng để truyền dữ liệu ra view
            * điểm khác là TempData có thể lưu lại và hiển thị ở một trang sau đó và nó chỉ biến mất khi người dùng đã "đọc" nó
            */
            TempData[STATUS.Success] = MSG.MsgCode4;
            return RedirectToAction(ActionNameConsts.Details, ControllerConsts.Order, new { orderId = OrderVM.OrderHeader.Id });
        }

        /// <summary>
        /// Get đơn hàng với điều kiện
        /// </summary>
        /// <param name="status"> trạng thái của đơn hàng</param>
        /// <returns></returns>
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            // filter data by roles
            IEnumerable<OrderHeader>  orderHeaders = FitlerDataByRole();

            // filter data by status
            orderHeaders = _orderService.FilterByStatus(orderHeaders, status);

            return Json(new { data = orderHeaders });
        }

        private DataTable GetProductsDetail(string orderStatus)
        {
            IEnumerable<OrderHeader> orders = FitlerDataByRole();
            orders = _orderService.FilterByStatus(orders, orderStatus);

            // Nên tạo constants cho magic string
            DataTable dtProduct = new DataTable("ProductDetails");
            dtProduct.Columns.AddRange(new DataColumn[4] { new DataColumn("ProductID"),
                                            new DataColumn("ProductName"),
                                            new DataColumn("Price"),
                                            new DataColumn("ProductDescription") });
            foreach (var order in orders)
            {
                dtProduct.Rows.Add(order.Id, order.Name, order.OrderTotal, order.PaymentStatus);
            }

            return dtProduct;
        }

        [HttpPost]
        public IActionResult ExporDataToFile()
        {
            FileContentResult rs;
            var dictioneryexportType = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            var exportType = dictioneryexportType["Export"];
            var orderStatus = dictioneryexportType["Status"];
            var products = GetProductsDetail(orderStatus);
            switch (exportType)
            {
                case "Excel":
                    return ExportToExcel(products);
            }

            return null;
        }

        /// <summary>
        /// Có thể viết hàm common để auto detect column
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private FileContentResult ExportToExcel(DataTable items)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Products");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "ProductID";
                worksheet.Cell(currentRow, 2).Value = "ProductName";
                worksheet.Cell(currentRow, 3).Value = "Price";
                worksheet.Cell(currentRow, 4).Value = "ProductDescription";

                for (int i = 0; i < items.Rows.Count; i++)
                {
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = items.Rows[i]["ProductID"];
                        worksheet.Cell(currentRow, 2).Value = items.Rows[i]["ProductName"];
                        worksheet.Cell(currentRow, 3).Value = items.Rows[i]["Price"];
                        worksheet.Cell(currentRow, 4).Value = items.Rows[i]["ProductDescription"];

                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "users.xlsx");
                }
            }
        }

        /// <summary>
        /// Nên move ra service
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrderHeader> FitlerDataByRole()
        {
            IEnumerable<OrderHeader> orderHeaders;

            // Check role to get data
            if (User.IsInRole(StatusData.Role_Admin) || User.IsInRole(StatusData.Role_Employee))
                orderHeaders = _unitOfWork.OrderHeaderRepo.GetAll(includeProperties: Entity.ApplicationUser);
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeaderRepo.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: Entity.ApplicationUser);
            }

            return orderHeaders;
        }
        #endregion
    }
}
