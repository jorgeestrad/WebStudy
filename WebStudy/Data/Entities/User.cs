namespace WebStudy.Data.Entities
{
    using Microsoft.AspNetCore.Identity;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class User :IdentityUser
    {
        [Display(Name = "FirstName")]
        [Required(ErrorMessage = "FirstNameRequired")]
        [MaxLength(50, ErrorMessage = "FirstNameMaxLength")]
        public string FirstName { get; set; }


        [Display(Name = "LastName")]
        [Required(ErrorMessage = "LastNameRequired")]
        [MaxLength(50, ErrorMessage = "LastNameMaxLength")]
        public string LastName { get; set; }

        [Display(Name = "PhoneNumber")]
        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [MaxLength(20, ErrorMessage = "The field {0} only can contains a maximum of {1} characters lenght.")]
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }

        public int CityId { get; set; }

        [Display(Name = "City")]
        public City City { get; set; }

        [Display(Name = "Image")]
        public string ImagePath { get; set; }

        [JsonIgnore]
        public UserType UserType { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(ImagePath))
                {
                    return "noimage";
                }
                return string.Format(
                    "http://190.147.168.232/gallery/{0}",
                    ImagePath.Substring(1));
            }

        }

        [Display(Name = "User")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        [NotMapped]
        [Display(Name = "IsAdmin")]
        public bool IsAdmin { get; set; }
    }
}
