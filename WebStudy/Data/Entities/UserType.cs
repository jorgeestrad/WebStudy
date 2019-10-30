namespace WebStudy.Data.Entities
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("UserType", Schema = "Security")]
    public class UserType :IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "RequiredName")]
        [MaxLength(50, ErrorMessage = "MaxLengthName")]
        [Display(Name = "DisplayName")]
        public string Name { get; set; }
    }
}
