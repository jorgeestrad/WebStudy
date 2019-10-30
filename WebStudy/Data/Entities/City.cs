namespace WebStudy.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Cities", Schema = "Masters")]
    public class City : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "CountryRequired")]
        public Country Country { get; set; }

        [Display(Name = "Name")]
        [MaxLength(50, ErrorMessage = "NameMaxLength")]
        [Required(ErrorMessage = "NameRequired")]
        public string Name { get; set; }
    }
}
