using BookStoreManagement.DataAccess.Data;
using BookStoreManagement.DataAccess.Repository.IRepository;
using BookStoreManagement.Models;

namespace BookStoreManagement.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
