using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasic.Models;

namespace WebApiBasic.Dtos
{
    public class ReadPatientWithIdDto
    {

        public Guid PatientId { get; set; }
        public string Name { get; set; }
        public double NationalCode { get; set; }
        public DateTime Birthday { get; set; }

        public List<VisitReadDto> Visits { get; set; }
        

    }
}
