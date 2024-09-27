using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;
using API.Dtos;
using AutoMapper;




namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;



        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;

        }

        [HttpGet]
        // [Authorize(Roles = "Admin , Management")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            var userDtos = _mapper.Map<List<UserDto>>(users);
            return Ok(userDtos);
        }

        
        [HttpGet("{id}")]
      //  [Authorize(Roles = "Admin, Management")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }




        [HttpPut("{id}")]
      //  [Authorize(Roles = "Admin, Management")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var userEntity = await _userService.GetUserByIdAsync(id);
            if (userEntity == null)
            {
                return NotFound("User not found.");
            }

    
            _mapper.Map(userDto, userEntity);
            await _userService.UpdateUserAsync(userEntity);

            return NoContent(); 
        }


        
        [HttpDelete("{id}")]
       //   [Authorize(Roles = "Auditor, Management")] 
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }


    }
}
