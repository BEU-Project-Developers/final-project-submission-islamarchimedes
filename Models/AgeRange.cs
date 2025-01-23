using System.ComponentModel.DataAnnotations;

namespace PharmaPulseApp.Models
{
    public class AgeRange
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Age range name is required")]
        [Display(Name = "Age Range")]
        [StringLength(100, ErrorMessage = "Age range name is too long")]
        public string Name { get; set; }


        public List<Medicine> Medicines { get; set; }
    }
}
