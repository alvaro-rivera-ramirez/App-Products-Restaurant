using System.ComponentModel.DataAnnotations;

namespace ApiRestaurant.Models
{
    public class Product
    {
        
        [Key]
        public int id_producto { get; set; }
        public required string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string name_file { get; set; }
        public bool available { get; set; }
        public int categoryId { get;set; }
    }
}
