
using System.Security.Claims;
using API.Dtos;
using API.Helper;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepository;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IUserRepository userRepository, IRoleRepository roleRepository, AppDbContext context, IMapper mapper)
        {
            _authService = authService;
            _userRepo = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _context = context;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userObj)
        {
            if (userObj == null)
                return BadRequest();

            var user = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(x => x.Email == userObj.Email);

            if (user == null)
                return NotFound(new { Message = "User Not Found!" });

            if (!user.IsActive)
            {
                return BadRequest(new { Message = "Account is disabled and needs to contact support" });
            }

            // Verify password
            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }

            // Generate new tokens
            var newAccessToken = _authService.CreateJwt(user); // Generate the JWT token here
            var newRefreshToken = _authService.CreateRefreshToken();

            user.SetTokens(newAccessToken); // Set the generated access token
            user.SetRefreshToken(newRefreshToken, DateTime.Now.AddDays(5));
            await _context.SaveChangesAsync();

            return Ok(new TokenDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }



        // POST: api/user
        [HttpPost("register")]
        [Authorize(Roles = "Admin , Management")] 
          public async Task<ActionResult<UserDto>> Register([FromBody] UserDto userObj)
        {
            if (userObj == null)
                return BadRequest(new { Message = "User data is null" });


            if (await _authService.CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email Already Exists!" });


            var role = await _roleRepository.GetRoleByIdAsync(userObj.RoleId);
            if (role == null)
                return BadRequest(new { Message = "Invalid role" });


            var user = _mapper.Map<User>(userObj);
            user.IsActive = true;
            user.Password = PasswordHasher.HashPassword(userObj.Password);


            await _userRepo.AddAsync(user);

            return Ok(new
            {
                Message = "User Registered!"
            });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenApiDto)
        {
            if (tokenApiDto == null || string.IsNullOrEmpty(tokenApiDto.AccessToken) || string.IsNullOrEmpty(tokenApiDto.RefreshToken))
                return BadRequest("Invalid client request");

            var principal = _authService.GetPrincipleFromExpiredToken(tokenApiDto.AccessToken);

            // Access the email claim
            var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailClaim))
            {
                return BadRequest("Email claim not found in token");
            }

       
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailClaim);
            if (user == null)
                return BadRequest("User not found");

            // Validate 
            if (user.RefreshToken != tokenApiDto.RefreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
                return BadRequest("Invalid refresh token");


            var newAccessToken = _authService.CreateJwt(user);
            var newRefreshToken = _authService.CreateRefreshToken();


            user.SetRefreshToken(newRefreshToken, DateTime.Now.AddDays(5));
            await _context.SaveChangesAsync();

            return Ok(new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Find the user by ID
            var user = await _context.Users.FindAsync(resetPasswordDto.Id);
            if (user == null)
            {
                return NotFound(new { Message = "User does not exist." });
            }

            // Verify the current password
            if (!PasswordHasher.VerifyPassword(resetPasswordDto.CurrentPassword, user.Password))
            {
                return BadRequest(new { Message = "Current password is incorrect." });
            }

            // Check if the new password is the same as the current password
            if (resetPasswordDto.NewPassword == resetPasswordDto.CurrentPassword)
            {
                return BadRequest(new { Message = "New password cannot be the same as the current password." });
            }

            // Update the user's password
            user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Password reset successful!" });
        }

       
      
    }
}