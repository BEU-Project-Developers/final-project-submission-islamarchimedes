using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace PharmaPulseApp.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Medicine Picture")]
        public string MedicinePictureUrl { get; set; }


        [Required(ErrorMessage = "Medicine's name is required")]
        [Display(Name = "Name")]
        [StringLength(100, ErrorMessage = "Medicine's name is too long")]
        public string Name { get; set; }


        [Display(Name = "Fabrication Date")]
        [DataType(DataType.Date)]
        public DateTime FabDate { get; set; }


        [Display(Name = "Expiration Date")]
        [DataType(DataType.Date)]
        public DateTime ExpDate { get; set; }


        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Price")]
        [DataType(DataType.Currency, ErrorMessage = "Price format is not valid ")]
        public double Price { get; set; }


        [Required(ErrorMessage = "Amount is required")]
        [Display(Name = "Amount")]
        public int Amount { get; set; }


        [Display(Name = "Description")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Manufacturer's name is required")]
        [Display(Name = "Manufacturer Name")]
        [StringLength(100, ErrorMessage = "Manufacturer's name is too long")]
        public string ManufacturerName { get; set; }


        [Required]
        public int AgeRangeId { get; set; }
        [ForeignKey("AgeRangeId")]
        public AgeRange AgeRange { get; set; }


        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }


        public List<Pharmacy_Medicine> Pharmacy_Medicines { get; set; }
    }
}
