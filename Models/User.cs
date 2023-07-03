using System.ComponentModel.DataAnnotations;

namespace ApiRestaurant.Models
{
    public class User
    {
        [Key]
        public int id_user { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string password { get; set; }

    }
}
