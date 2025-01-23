using System.ComponentModel.DataAnnotations;

namespace PharmaPulseApp.Models
{
    public class Request
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int MedicineId { get; set; }

        public string Message { get; set; }

        public string PhoneNumber { get; set; }
    }
}
