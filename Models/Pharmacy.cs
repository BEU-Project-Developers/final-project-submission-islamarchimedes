using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace PharmaPulseApp.Models
{
    public class Pharmacy
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Pharmacy Picture")]
        public string PharmacyPictureUrl { get; set; }


        [Required(ErrorMessage = "Pharmacy's name is required")]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "Pharmacy's name is too long")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Address is required")]
        [Display(Name = "Address")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }


        public List<Pharmacy_Medicine> Pharmacy_Medicines { get; set; }
    }
}
