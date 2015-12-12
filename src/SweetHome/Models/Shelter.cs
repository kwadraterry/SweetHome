using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace SweetHome.Models
{
    public class Shelter
    {
        [Key]
        public virtual int Id { get; protected set; }
        [Required]
        public virtual string Name { get; set; }
        [StringLength(160)]
        public virtual string OwnerName { get; set; }
        [StringLength(160)]
        public virtual string Address { get; set; }
        public virtual string Info { get; set; }
        public virtual string VKGroup { get; set; }
        public virtual string Image { get; set; }
        public virtual string URL { get; set; }
        public virtual IList<ShelterAnimal> Animals { get; set; }
        public Shelter()
        {
            Animals = new List<ShelterAnimal>();
            this._PhoneNumbers = new List<string>();
        }
        protected string PhonesSerialized
        {
            get
            {
                return JsonConvert.SerializeObject( _PhoneNumbers);
            }
            set
            {
                 _PhoneNumbers = JsonConvert.DeserializeObject<IList<string>>(value);
            }
        }
        
        public virtual IList<string> PhoneNumbers
        {
            get
            {
                return _PhoneNumbers;
            }
            set
            {
                _PhoneNumbers = value;
            }
        }
        
        
        private IList<string> _PhoneNumbers;
        public class ShelterMap: ClassMap<Shelter>
    	{
    		public ShelterMap()
    		{
    			Id(shelter => shelter.Id).GeneratedBy.Increment();
    			Map(shelter => shelter.Name).Not.Nullable();
    			Map(shelter => shelter.Address).Length(160);
                Map(shelter => shelter.OwnerName).Length(160);
    			Map(animal => animal.PhonesSerialized).CustomSqlType("text").Not.Nullable();
                Map(shelter => shelter.Info).CustomSqlType("text");
                Map(shelter => shelter.VKGroup).Length(500);
                Map(shelter => shelter.URL).Length(500);
                Map(shelter => shelter.Image).Length(500);
                HasMany(shelter => shelter.Animals);
    			Table("shelters");
    		}
    	}
    }
}