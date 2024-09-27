namespace Infrastructure.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Entities;

    using Core.Interfaces;
    using Microsoft.EntityFrameworkCore;

    namespace Infrastructure.Repositories
    {
        public class WarehouseRepository : IWarehouseRepository
        {
            private readonly AppDbContext _context;

            public WarehouseRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task<IEnumerable<Warehouse>> GetAllWarehousesAsync()
            {
                return await _context.Warehouses
                    .Include(w => w.City)    
                    .ToListAsync();
            }


            public async Task<Warehouse> GetWarehouseByIdAsync(int id)
            {
                return await _context.Warehouses
                    .Include(w => w.City)
                    .Include(w => w.Country)
                    .FirstOrDefaultAsync(w => w.Id == id);
            }

   
            public async Task AddWarehouseAsync(Warehouse warehouse)
            {
                await _context.Warehouses.AddAsync(warehouse);
                await _context.SaveChangesAsync();
            }


            public async Task UpdateWarehouseAsync(Warehouse warehouse)
            {
                _context.Entry(warehouse).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }


            public async Task<bool> WarehouseExistsAsync(int id)
            {
                return await _context.Warehouses.AnyAsync(w => w.Id == id);
            }

        }
    }

}