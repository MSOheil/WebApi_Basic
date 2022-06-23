using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBasic.Models
{
    public class Patient
    {
        [Key]
        public Guid PatientId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double NationalCode { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public bool IsActive { get; set; }
        //Navigation Property
        public IEnumerable<Visit> Visits { get; set; }

    }
}
