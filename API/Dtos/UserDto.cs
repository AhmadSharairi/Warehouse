using System.Text.Json.Serialization;


namespace API.Dtos
{
    public class UserDto
    {

        public int Id { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }
        public bool IsActive { get; set; }

        public string RoleName { get; set; }
        [JsonIgnore]
        public int RoleId { get; set; }


        [JsonIgnore]
        public string Password { get; set; }

    }
}