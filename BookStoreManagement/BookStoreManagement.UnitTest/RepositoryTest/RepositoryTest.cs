using BookStoreManagement.DataAccess.Repository;
using BookStoreManagement.DataAccess.Repository.IRepository;
using BookStoreManagement.Models;
using BookStoreManagement.Web.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.UnitTest.RepositoryTest
{
    public class RepositoryTest
    {
        private Category _category;
        private List<Category> _categories;
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<ICategoryRepository> _categoryRepository;

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

            // Setup Repository
            _categoryRepository = new Mock<ICategoryRepository>();
            _categoryRepository.Setup(s => s.GetAll(null, null)).Returns(new List<Category> { 
                new Category
                {
                    Id = 2,
                    Name = "Cate 2",
                    DisplayOrder = 2
                } 
            });
            //_categoryRepository.Setup(s => s.GetFirstOrDefault(s => s.Id > 0, null, true)).Returns(_category);

            // Setup UnitOfWork
            //_unitOfWork = new Mock<IUnitOfWork>();
            //_unitOfWork.Setup(u => u.CategoryRepo).Returns(_categoryRepository.Object);
        }

        [Test]
        public void CreateCategory_CategoryCreated_ReturnEmpty()
        {

            //Assert.That(result, Is.EqualTo("1,2,3"));
        }
    }
}
