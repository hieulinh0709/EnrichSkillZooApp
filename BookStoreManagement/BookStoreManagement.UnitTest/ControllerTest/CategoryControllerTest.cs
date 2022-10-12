using BookStoreManagement.DataAccess.Repository.IRepository;
using BookStoreManagement.Models;
using BookStoreManagement.UnitTest.Constants;
using BookStoreManagement.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using Stripe;
using System.Linq.Expressions;

namespace BookStoreManagement.UnitTest.ControllerTest
{
    [TestFixture]
    public class CategoryControllerTest
    {
        private Category _category;
        private List<Category> _categories;
        private Mock<ICategoryRepository> _categoryRepository;
        private Mock<IUnitOfWork> _unitOfWork;
        private TempDataDictionary _tempData;
        private CategoryController _categoryController;

        [SetUp]
        public void Setup()
        {
            SetipCategoryData();

            SetupTempData();

            SetupRepository();

            SetupUnitOfWork();

            SetupController();
        }

        /// <summary>
        /// CreateSuccessed
        /// </summary>
        [Test]
        public void CreateCategory_CreateSuccessed_ReturnViewName()
        {
            // Act
            var result = (RedirectToActionResult) _categoryController.Create(_category);

            // Assert
            Assert.That(result?.ActionName, Is.EqualTo(Common.ActionName.Index));

            _unitOfWork.Verify(s => s.CategoryRepo.Add(_category));
        }

        /// <summary>
        /// CreateFailed
        /// </summary>
        [Test]
        public void CreateCategory_CreateFailed_ReturnViewName()
        {
            _categoryController.ModelState.AddModelError("Name", "Required");

            var result = _categoryController.Create(_category) as ViewResult;
            Assert.That(result?.ViewName, Is.EqualTo(Common.ActionName.Create));
        }

        /// <summary>
        /// EditSuccessed. Test case for getting category with id to check successfully
        /// </summary>
        /// <param name="id"></param>
        [TestCase(1)] // Test case with parameter for function
        public void GetCategory_EditSuccessed_ReturnViewName(int? id)
        {
            var result = _categoryController.Edit(id) as ViewResult;

            Assert.That(result?.ViewName, Is.EqualTo(Common.ActionName.Edit));

            _unitOfWork.Verify(s => s.CategoryRepo.GetFirstOrDefault(s => s.Id == id, null, true));
        }

        /// <summary>
        /// EditFailed. Test case for getting category with id to check NotFound
        /// </summary>
        /// <param name="id"></param>
        [TestCase(null)]
        [TestCase(0)]
        public void GetCategory_EditFailed_ReturnNotFound(int? id)
        {
            // Setup repository data is null 
            _categoryRepository.Setup(s => s.GetFirstOrDefault(It.IsAny<Expression<Func<Category, bool>>>(), null, true));
            _unitOfWork.Setup(u => u.CategoryRepo).Returns(_categoryRepository.Object);

            var result = _categoryController.Edit(id) as ViewResult;

            // Check return view is null
            Assert.That(result?.ViewName, Is.Null);
        }

        /// <summary>
        /// Object array for test
        /// </summary>
        private static readonly object[] TestCases =
        {
            new Category { Id = 1, Name = "edited id 1", DisplayOrder = 2 }
        };

        /// <summary>
        /// Edit Successed
        /// </summary>
        /// <param name="category"></param>
        [TestCaseSource(nameof(TestCases))] // Test case with parameter for function
        public void EditCategory_UpdateSuccessed_ReturnViewName(Category category)
        {
            var result = (RedirectToActionResult) _categoryController.Edit(category);

            Assert.That(actual: result?.ActionName, Is.EqualTo(Common.ActionName.Index));

            _unitOfWork.Verify(s => s.CategoryRepo.Update(category));
        }

        /// <summary>
        /// Edit Failed
        /// </summary>
        /// <param name="category"></param>
        [TestCaseSource(nameof(TestCases))] // Test case with parameter for function
        public void EditCategory_UpdateFailed_ReturnError(Category category)
        {
            _categoryController.ModelState.AddModelError("Name", "Required");
            var result = _categoryController.Edit(category) as ViewResult;

            Assert.That(actual: result?.ViewName, Is.EqualTo(Common.ActionName.Edit));
        }

        public void SetipCategoryData()
        {
            _category = new Category { Id = 1, Name = "Cate 1", DisplayOrder = 1 };
            _categories = new List<Category>
            {
                _category,
                new Category { Id = 2, Name = "Cate 2", DisplayOrder = 2 }
            };
        }
        public void SetupTempData()
        {
            // Setup TempData
            var httpContext = new DefaultHttpContext();
            _tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _tempData["Info"] = "NUnit test";
        }
        public void SetupRepository()
        {
            // Setup Repository
            _categoryRepository = new Mock<ICategoryRepository>();

            //Setup for GetAll and GetFirstOrDefault function
            _categoryRepository.Setup(s => s.GetAll(null, null)).Returns(_categories);
            _categoryRepository.Setup(s => s.GetFirstOrDefault(It.IsAny<Expression<Func<Category, bool>>>(), null, true)).Returns(_category);
        }
        public void SetupUnitOfWork()
        {
            // Setup UnitOfWork to dependency injection for controller
            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(u => u.CategoryRepo).Returns(_categoryRepository.Object);
        }
        public void SetupController()
        {
            // New instance for controller
            _categoryController = new CategoryController(_unitOfWork.Object);
            _categoryController.TempData = _tempData;
        }
    }
}
