using BookStoreManagement.DataAccess.Repository.IRepository;
using BookStoreManagement.Models;
using BookStoreManagement.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System.Linq.Expressions;

namespace BookStoreManagement.UnitTest.ControllerTest
{
    [TestFixture]
    public class CategoryControllerTest
    {
        private CategoryController _categoryController;
        private List<Category> _categories;
        private Category _category;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ICategoryRepository> _categoryRepository;
        private TempDataDictionary _tempData;

        [SetUp]
        public void Setup()
        {
            // New object for Category
            _category = new Category
            {
                Id = 1,
                Name = "Cate 1",
                DisplayOrder = 1
            };

            // New object list for Category
            _categories = new List<Category>
            {
                _category,
                new Category
                {
                    Id = 2,
                    Name = "Cate 2",
                    DisplayOrder = 2
                }
            };

            // Setup TempData
            var httpContext = new DefaultHttpContext();
            _tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _tempData["Info"] = "NUnit test";

            // Setup Repository
            _categoryRepository = new Mock<ICategoryRepository>();
            _categoryRepository.Setup(s => s.GetAll(null, null)).Returns(_categories);
            _categoryRepository.Setup(s => s.GetFirstOrDefault(It.IsAny<Expression<Func<Category, bool>>>(), null, true)).Returns(_category);

            // Setup UnitOfWork
            _unitOfWork = new Mock<IUnitOfWork>();
            _unitOfWork.Setup(u => u.CategoryRepo).Returns(_categoryRepository.Object);


            _categoryController = new CategoryController(_unitOfWork.Object);
        }

        [Test]
        public void CreateCategory_WhenCalled_CreateTheCategoryFromDb()
        {
            var controller = new CategoryController(_unitOfWork.Object);

            controller.TempData = _tempData;
            controller.Create(_category);

            _unitOfWork.Verify(s => s.CategoryRepo.Add(_category));
        }

        [TestCase(1)] // Test case with parameter for function
        public void GetCategory_WhenCalled_GetTheCategoryFromDb(int? id)
        {
            _categoryController.TempData = _tempData;
            _categoryController.Edit(id);

            _unitOfWork.Verify(s => s.CategoryRepo.GetFirstOrDefault(s => s.Id == id, null, true));
        }
    }
}
