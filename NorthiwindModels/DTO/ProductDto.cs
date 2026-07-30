namespace NortwindReporting.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Product { get; set; } = string.Empty;

        public decimal? Price { get; set; }
    }
}
