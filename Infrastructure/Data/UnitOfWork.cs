using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;


        public UnitOfWork(AppDbContext context, IWarehouseRepository warehouseRepository, IUserRepository userRepository)
        {
            _context = context;
            WarehouseRepository = warehouseRepository;
            UserRepository = userRepository;
        }

        public IWarehouseRepository WarehouseRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }


        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync(); 
        }

    
        public void Dispose() 
        {
            _context.Dispose();
        }

    
    }
}
