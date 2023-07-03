using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ApiRestaurant.DTO
{
    public class LoginDTO
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
