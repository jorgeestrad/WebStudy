namespace WebStudy.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    public class CityViewModel
    {
        public int CityId { get; set; }
        public int CountryId { get; set; }

        [Required(ErrorMessage = "CityRequired")]
        [MaxLength(50, ErrorMessage = "CityMaxLength")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
