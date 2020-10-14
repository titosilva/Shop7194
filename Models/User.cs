using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    [Table("User")]
    public class User{
        [Key]
        public int userId {get; set;}

        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string userName {get; set;}

        [Required]
        [MaxLength(20)]
        [MinLength(3)]
        public string password {get; set;}

        public string role {get; set;}
    }
}