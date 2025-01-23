using System.ComponentModel.DataAnnotations;

namespace PharmaPulseApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Category name is required")]
        [Display(Name = "Category Name")]
        [StringLength(100, ErrorMessage = "Category name is too long")]
        public string Name { get; set; }


        public List<Medicine> Medicines { get; set; }
    }
}
