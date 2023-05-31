using BulkyBook.DataAccess.Repository.IRepository;
namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {   private ApplicationDbContext _context;
        
        public UnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
            Category = new CategoryRepository(_context);
            CoverType = new CoverRepository(_context);
            Product = new ProductRepository(_context);
            Company = new CompanyRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            OrderDetails = new OrderDetailsRepository(_context);
            OrderHeader = new OrderHeaderRepository(_context);  
        }
        public ICategoryRepository Category { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
