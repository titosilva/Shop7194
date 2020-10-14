using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("Category")]
    public class Category{
        [Key]
        [Column("Id_cat")]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [MaxLength(60, ErrorMessage = "Max. length of this field is 60 characters")]
        [MinLength(3, ErrorMessage = "Min. length of this field is 3 characters")]
        public string Title { get; set; }
    }
}