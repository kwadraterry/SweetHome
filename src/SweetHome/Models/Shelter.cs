using System.ComponentModel.DataAnnotations;

namespace SweetHome.Models
{
    public class Shelter
    {
        public int ShelterId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}