using System.ComponentModel.DataAnnotations;

namespace ApiRestaurant.Models
{
    public class Category
    {
        [Key]
        public int id_categoria { get; set; }
        public required string name { get; set; }

    }
}
