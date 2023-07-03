namespace ApiRestaurant.DTO
{
    public class ListProductDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string name_file { get; set; }
        public bool available { get; set; }
        public int categoryId { get; set; }
    }
}
