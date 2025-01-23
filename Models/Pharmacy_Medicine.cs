using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PharmaPulseApp.Models
{
    public class Pharmacy_Medicine
    {
        [Key, Column(Order = 0)]
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }


        [Key, Column(Order = 1)]
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
    }
}
