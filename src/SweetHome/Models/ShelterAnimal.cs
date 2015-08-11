using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json;
using FluentNHibernate.Mapping;

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
    public enum Gender
    {
        Male,
        Female,
        Unknown
    }
    public class ShelterAnimal
    {
        [Key]
        public virtual int Id { get; protected set; }
        [Required]
        public virtual string Name { get; set; }
        [Required]
        public virtual AnimalType AnimalType { get; set; }
        [DataType(DataType.Date)]
        public virtual DateTime? BirthDay { get; set; }
        [Required]
        public virtual PlaceType PlaceType { get; set; }
        public virtual Shelter Shelter { get; set; }
        [Display(Name = "Полностью здоров")]
        public virtual bool IsHealthy { get; set; }
        [Display(Name = "Подходит для содержания в квартире")]
        public virtual bool IsForFlat { get; set; }
        [Display(Name = "Подходит для содержания в частном доме")]
        public virtual bool IsForHome { get; set; }
        [Display(Name = "Приучен к выгулу/туалету")]
        public virtual bool Toilet { get; set; }
        public virtual Color Color { get; set; }
        public virtual Size Size { get; set; }
        [StringLength(500)]
        public virtual string Info { get; set; }
        public virtual bool IsHappy { get; set; }
        [Required]
        [DataType(DataType.DateTime)]     
        public virtual DateTime Created { get; set; }
        public virtual Gender Gender { get; set; }
        protected string ImagesSerialized
        {
            get
            {
                return JsonConvert.SerializeObject(_Images);
            }
            set
            {
                _Images = JsonConvert.DeserializeObject<IList<string>>(value);
            }
        }
        public virtual IList<string> Images
        {
            get
            {
                return _Images;
            }
            set
            {
                _Images = value;
            }
        }
        private IList<string> _Images;
        
        public ShelterAnimal()
        {
            this.IsHappy = false;
            this._Images = new List<string>();
        }
        
        public class ShelterAnimalMap: ClassMap<ShelterAnimal>
        {
            public ShelterAnimalMap()
    		{
    			Id(shelter => shelter.Id).GeneratedBy.Increment();
    			Map(animal => animal.Name).Not.Nullable();
    			Map(animal => animal.AnimalType).Not.Nullable().CustomType<AnimalType>();
                Map(animal => animal.BirthDay).CustomSqlType("date");
                Map(animal => animal.Color).CustomType<Color>();
                Map(animal => animal.Created).Not.Nullable();
                Map(animal => animal.ImagesSerialized).CustomSqlType("text");
                Map(animal => animal.Info).CustomSqlType("text");
                Map(animal => animal.IsForFlat);
                Map(animal => animal.IsForHome);
                Map(animal => animal.IsHappy);
                Map(animal => animal.IsHealthy);
                Map(animal => animal.PlaceType).Not.Nullable().CustomType<PlaceType>();
                Map(animal => animal.Size).CustomType<Size>();
                Map(animal => animal.Toilet);
                Map(animal => animal.Gender).CustomType<Gender>().Not.Nullable();
                References(animal => animal.Shelter);
    			Table("animals");
    		}
        }
    }
}