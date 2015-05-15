using System.ComponentModel.DataAnnotations;

namespace SweetHome.Models
{
    public class Shelter
    {
        [Key]
        public int ShelterId { get; set; }
        [Required]
        public string Name { get; set; }
        [StringLength(160)]
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}