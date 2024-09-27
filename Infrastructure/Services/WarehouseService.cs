using Core.Entities;
using Core.Interfaces;

namespace Core.Services
{
    public class WarehouseService : IWarehouseService
    {

        private readonly IUnitOfWork _unitOfWork;

        public WarehouseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<Warehouse>> GetAllWarehousesAsync()
        {
            var warehouses = await _unitOfWork.WarehouseRepository.GetAllWarehousesAsync();
            if (warehouses == null)
            {
                throw new InvalidOperationException("No warehouses found.");
            }
            return warehouses;
        }


        public async Task<Warehouse> GetWarehouseByIdAsync(int id)
        {
            return await _unitOfWork.WarehouseRepository.GetWarehouseByIdAsync(id);
        }
        public async Task<bool> WarehouseExistsAsync(int id)
        {
            return await _unitOfWork.WarehouseRepository.WarehouseExistsAsync(id);
        }

        public async Task UpdateWarehouseAsync(Warehouse warehouse)
        {
            await _unitOfWork.WarehouseRepository.UpdateWarehouseAsync(warehouse);
        }


        public async Task AddWarehouseAsync(Warehouse warehouse)
        {
            await _unitOfWork.WarehouseRepository.AddWarehouseAsync(warehouse);
        }





    }
}
