using System.ComponentModel.DataAnnotations;

namespace SweetHome.Models
{
    public enum PlaceType
    {
        Shelter,
        HomeShelter,
        Home
    }
    public enum AnimalType
    {
        Dog,
        Cat,
        Other
    }
    public enum Color
    {
        Dog,
        Blond,	
        Varicoloured,
        Dark       
    }
    public enum Size
    {
        Cat,
        Small,	
        Medium,
        Large        
    }
    public class ShelterAnimal
    {
        [Key]
        public int ShelterAnimalId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public AnimalType AnimalType { get; set; }
        [DataType(DataType.Date)]
        public string BirthDay { get; set; }
        [Required]
        public PlaceType PlaceType { get; set; }
        public int ShelterId { get; set; }
        [Display(Name = "Полностью здоров")]
        public bool IsHealth { get; set; }
        [Display(Name = "Подходит для содержания в квартире")]
        public bool IsForFlat { get; set; }
        [Display(Name = "Подходит для содержания в частном доме")]
        public bool IsForHome { get; set; }
        [Display(Name = "Приучен к выгулу/туалету")]
        public bool Toilet { get; set; }
        public Color Color { get; set; }
        public Size Size { get; set; }
        [StringLength(500)]
        public string Info { get; set; }
        public bool IsHappy { get; set; }
        [Required]
        [DataType(DataType.DateTime)]     
        public string Created { get; set; }
        
        public virtual Shelter Shelter { get; set; }
    }
}