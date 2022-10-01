using BookStoreManagement.DataAccess.Data;
using BookStoreManagement.DataAccess.Repository.IRepository;
using BookStoreManagement.Models;

namespace BookStoreManagement.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
