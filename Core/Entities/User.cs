using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class User : BaseEntity
    {
      [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

  
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Must be between 3 and 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; } = true;

        public string Token { get; private set; } = string.Empty;
        public string RefreshToken { get; private set; } = string.Empty;


        public DateTime RefreshTokenExpireTime { get; private set; }

        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public string ResetPasswordToken { get; private set; } = string.Empty;

        public DateTime ResetPasswordExpiry  { get; private set; } 



        public void SetRefreshToken(string refreshToken, DateTime expireTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpireTime = expireTime;
        }


        public void SetTokens(string accessToken)
        {
            Token = accessToken;

        }

      
    }
}
