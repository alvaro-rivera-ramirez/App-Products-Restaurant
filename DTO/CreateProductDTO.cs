namespace ApiRestaurant.DTO
{
    public class CreateProductDTO
    {
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public IFormFile name_file { get; set; }
        public bool available { get; set; }
        public int categoryId { get; set; }
    }
}
