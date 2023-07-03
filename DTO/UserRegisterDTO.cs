using System.ComponentModel.DataAnnotations;

namespace ApiRestaurant.DTO
{
    public class UserRegisterDTO
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string lastname { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
