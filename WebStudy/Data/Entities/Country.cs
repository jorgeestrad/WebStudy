namespace WebStudy.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Countries", Schema = "Masters")]
    public class Country : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [MaxLength(50, ErrorMessage = "NameMaxLength")]
        [Required(ErrorMessage = "NameRequired")]
        public string Name { get; set; }

        [Display(Name = "Cities")]
        public ICollection<City> Cities { get; set; }

        [Display(Name = "NCities")]
        public int NumberCities { get { return this.Cities == null ? 0 : this.Cities.Count; } }

    }
}
