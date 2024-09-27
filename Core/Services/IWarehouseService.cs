using Core.Entities;

namespace Core.Interfaces
{
    public interface IWarehouseService
    {
        Task<IEnumerable<Warehouse>> GetAllWarehousesAsync();
        Task<Warehouse> GetWarehouseByIdAsync(int id);
         Task<bool> WarehouseExistsAsync(int id);
         Task UpdateWarehouseAsync(Warehouse warehouse);

        Task AddWarehouseAsync(Warehouse warehouse);



    }
}
