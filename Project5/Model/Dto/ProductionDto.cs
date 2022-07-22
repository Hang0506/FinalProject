using System.ComponentModel.DataAnnotations;

namespace Project5.Model
{
    public class ProductionDto
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
    }
    public class DeteleDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
