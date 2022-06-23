using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBasic.Dtos
{
    public class CreatePatientDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double NationalCode { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public bool IsActive { get; set; }

    }
}
