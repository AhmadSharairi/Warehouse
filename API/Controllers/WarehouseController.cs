using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;
using API.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Core.DTOs;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;


        public WarehouseController(IWarehouseService warehouseService, IMapper mapper, AppDbContext context)
        {
            _warehouseService = warehouseService;
            _mapper = mapper;
            _context = context;
        }

     
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses()
        {
            var warehouses = await _warehouseService.GetAllWarehousesAsync();
            return Ok(warehouses);
        }

   
        [HttpGet("{id}")]
        public async Task<ActionResult<Warehouse>> GetWarehouse(int id)
        {
            var warehouse = await _warehouseService.GetWarehouseByIdAsync(id);

            if (warehouse == null)
            {
                return NotFound();
            }

            return Ok(warehouse);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouse(int id, [FromBody] Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                return BadRequest();
            }

            if (!await _warehouseService.WarehouseExistsAsync(id))
            {
                return NotFound();
            }

            await _warehouseService.UpdateWarehouseAsync(warehouse);

            return NoContent();
        }

        [HttpGet("items/count")]
        public async Task<IActionResult> GetTotalItemsCount()
        {
            try
            {
                var totalCount = await _context.Items.CountAsync();
                return Ok(totalCount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Warehouse>> PostWarehouse([FromBody] WarehouseCreateDto warehouseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Name == warehouseDto.CountryName);
            if (country == null)
            {
                country = new Country { Name = warehouseDto.CountryName };
                await _context.Countries.AddAsync(country);
                await _context.SaveChangesAsync();
            }


            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == warehouseDto.CityName);
            if (city == null)
            {
                city = new City
                {
                    Name = warehouseDto.CityName,
                    CountryId = country.Id
                };
                await _context.Cities.AddAsync(city);
                await _context.SaveChangesAsync();
            }

            // Map DTO
            var warehouse = _mapper.Map<Warehouse>(warehouseDto);
            warehouse.City = city;
            warehouse.Country = country;


            await _warehouseService.AddWarehouseAsync(warehouse);

            return Ok(new { message = "Added Warehouse" });
        }


        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<WarehouseInfoDto>>> GetAllWarehouseInfo()
        {
            var warehousesInfo = await _context.Warehouses
                .Include(w => w.City)
                .Include(w => w.Country)
                .Include(w => w.Items)
                .Select(w => new WarehouseInfoDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Address = w.Address,
                    Country = w.Country.Name,
                    City = w.City.Name,
                    ItemsCount = w.Items.Count(),
                    Status = w.Items.Count() > 0 ? "Available" : "Empty"
                })
                .ToListAsync();

            return Ok(warehousesInfo);
        }


        [HttpGet("{warehouseId}/items")]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetItemsByWarehouseId(int warehouseId)
        {

            var items = await _context.Items
                .Where(i => i.WarehouseId == warehouseId)
                .ToListAsync();

            if (items == null || !items.Any())
            {
                return NotFound();
            }

            var itemDtos = _mapper.Map<List<ItemDto>>(items);

            return Ok(itemDtos);
        }




    }
}

