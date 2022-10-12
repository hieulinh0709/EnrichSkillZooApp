using BookStoreManagement.DataAccess.Repository;
using BookStoreManagement.DataAccess.Repository.IRepository;
using BookStoreManagement.Models;
using BookStoreManagement.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Stripe;
using Stripe.Checkout;
using System.Collections;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Controller;
using BookStoreManagement.UnitTest.Constants;
using System.Net.Http;
using BookStoreManagement.Utility;

namespace BookStoreManagement.UnitTest.ControllerTest
{
    [TestFixture]
    public class OrderControllerTest
    {
        private Mock<IOrderHeaderRepository> _headerRepository;
        private Mock<IOrderDetailRepository> _detailRepository;
        private TempDataDictionary _tempData;
        private Mock<IUnitOfWork> _unitOfWork;
        private ControllerContext _controllerContext;
        private OrderController _orderController;
        private OrderHeader _orderHeader;
        private IEnumerable<OrderDetail> _orderDetails;
        private HttpContext _httpContext;

        [SetUp]
        public void Setup()
        {
            //Controller needs a controller context 
            SetUpControllerContext();

            // Init data
            InitData();

            // Setup TempData
            SetUpTempData();

            // Setup Repository
            SetUpRepository();

            // Setup UnitOfWork to dependency injection for controller
            SetupUnitOfWork();

            // Setup Controller
            SetUpController();

            // Setup Stripe service
            SetUpStripeService();
        }

        /// <summary>
        /// My description
        /// </summary>
        [Test]
        public void PayNow_PaySuccessfully_ReturnStatusCode200()
        {
            var rs = _orderController.Details_PAY_NOW() as StatusCodeResult;

            Assert.That(rs?.StatusCode, Is.EqualTo(200));
        }

        public void PayNow_PayFailed_ReturnStatusCodeNot200() { }

        [Test]
        public void PaymentConfirmation_PaymentSuccessfully_ReturnView()
        {
            var rs = _orderController.PaymentConfirmation(_orderHeader.Id) as ViewResult;

            Assert.That(rs?.ViewName, Is.EqualTo(Common.ViewName.PaymentConfirmation));
        }

        #region Start setup data
        /// <summary>
        /// My description
        /// </summary>
        public void SetUpControllerContext()
        {
            _httpContext = new DefaultHttpContext();
            _controllerContext = new ControllerContext()
            {
                HttpContext = _httpContext,
            };
        }
        /// <summary>
        /// My description
        /// </summary>
        public void SetUpController()
        {
            _orderController = new OrderController(_unitOfWork.Object)
            {
                ControllerContext = _controllerContext,
            };

            _orderController.OrderVM = new Models.ViewModels.OrderVM()
            {
                OrderHeader = new OrderHeader() { Id = 1, Name = "name 1" },
                OrderDetail = _orderDetails
            };
        }

        /// <summary>
        /// My description
        /// </summary>
        public void SetUpTempData()
        {
            _tempData = new TempDataDictionary(_httpContext, Mock.Of<ITempDataProvider>());
            _tempData["Info"] = "NUnit test";
        }
        /// <summary>
        /// My description
        /// </summary>
        public void InitData()
        {
            _orderHeader = new OrderHeader { Id = 1, Name = "name 1", PaymentStatus = StatusData.PaymentStatusDelayedPayment, SessionId = "cs_test_a1kxU9NpQgYn8C2ZwNdqniq5b6zKjk5yyFvE87Vn64rOjerrpQxh99qoT2" };
            _orderDetails = new List<OrderDetail>()
                {
                    new OrderDetail()
                    {
                        Id = 1,
                        OrderId = 1,
                        Count = 10,
                        ProductId = 1,
                        Product = new Models.Product
                        {
                            Id = 1,
                            Title = "title 1",
                            Price = 10,
                            ListPrice = 10,
                        },
                        Price = 20
                    }
                }.AsEnumerable();
        }
        /// <summary>
        /// My description
        /// </summary>
        public void SetUpRepository()
        {
            _headerRepository = new Mock<IOrderHeaderRepository>();
            _detailRepository = new Mock<IOrderDetailRepository>();

            // Setup data for _headerRepository's GetFirstOrDefault function with "ApplicationUser" param
            _headerRepository.Setup(s =>
            s.GetFirstOrDefault(It.IsAny<Expression<Func<OrderHeader, bool>>>(), "ApplicationUser", true))
                .Returns(_orderHeader);


            _headerRepository.Setup(s =>
            s.GetFirstOrDefault(It.IsAny<Expression<Func<OrderHeader, bool>>>(), null, true))
                .Returns(_orderHeader);

            // Setup data for _detailRepository's GetFirstOrDefault function 
            _detailRepository.Setup(s =>
            s.GetAll(It.IsAny<Expression<Func<OrderDetail, bool>>>(), "Product"))
                .Returns(_orderDetails);
        }

        /// <summary>
        /// My description
        /// </summary>
        public void SetupUnitOfWork()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(u => u.OrderHeaderRepo).Returns(_headerRepository.Object);
            _unitOfWork.Setup(u => u.OrderDetailRepo).Returns(_detailRepository.Object);
        }

        public void SetUpStripeService()
        {
            StripeConfiguration.ApiKey = Common.Stripe.SecretKey;
            _orderController._sessionService = new SessionService();
            _orderController._sessionLineItem = new SessionLineItemOptions();
        }
        #endregion End setup data
    }
}
