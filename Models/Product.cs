using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Products")]
    public class Product{
        [Key]
        public int Id {get; set;}

        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string name {get; set;}

        [Required]
        [Range(1, 1000, ErrorMessage="Price must be in range 1-1000")]
        public int price {get; set;}

        [Required]
        public int categoryId {get; set;}
        public Category category {get; set;}
    }
}