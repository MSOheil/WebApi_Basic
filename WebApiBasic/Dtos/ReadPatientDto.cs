using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBasic.Dtos
{
    public class ReadPatientDto
    {

        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public double NationalCode { get; set; }
        public DateTime Birthday { get; set; }
        public int visitCount { get; set; }
    }
}
