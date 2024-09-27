using Core.Entities;

namespace Core.Interfaces
{
    public interface IWarehouseRepository
    {

        //CRUD Operations 
         Task<IEnumerable<Warehouse>> GetAllWarehousesAsync();

        Task<Warehouse> GetWarehouseByIdAsync(int id);
       
        Task AddWarehouseAsync(Warehouse warehouse);
        Task UpdateWarehouseAsync(Warehouse warehouse);
        Task<bool> WarehouseExistsAsync(int id);


        
    }
}