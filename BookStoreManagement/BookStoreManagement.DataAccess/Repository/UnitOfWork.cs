using BookStoreManagement.DataAccess.Data;
using BookStoreManagement.DataAccess.Repository.IRepository;

namespace BookStoreManagement.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            CategoryRepo = new CategoryRepository(_db);
            CoverTypeRepo = new CoverTypeRepository(_db);
            ProductRepo = new ProductRepository(_db);
            CompanyRepo = new CompanyRepository(_db);
            ApplicationUserRepo = new ApplicationUserRepository(_db);
            ShoppingCartRepo = new ShoppingCartRepository(_db);
            OrderHeaderRepo = new OrderHeaderRepository(_db);
            OrderDetailRepo = new OrderDetailRepository(_db);
        }
        public ICategoryRepository CategoryRepo { get; private set; }
        public ICoverTypeRepository CoverTypeRepo { get; private set; }
        public IProductRepository ProductRepo { get; private set; }
        public ICompanyRepository CompanyRepo { get; private set; }

        public IShoppingCartRepository ShoppingCartRepo { get; private set; }

        public IApplicationUserRepository ApplicationUserRepo { get; private set; }
        public IOrderHeaderRepository OrderHeaderRepo { get; private set; }
        public IOrderDetailRepository OrderDetailRepo { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
