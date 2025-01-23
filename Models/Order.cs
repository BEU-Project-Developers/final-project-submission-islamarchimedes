using System.ComponentModel.DataAnnotations;

namespace PharmaPulseApp.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int MedicineId { get; set; }

        public int UserId { get; set; }

        [DataType(DataType.Currency)]
        public double MedicinePrice { get; set; }

        public int Amount { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
