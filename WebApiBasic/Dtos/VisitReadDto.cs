using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBasic.Dtos
{
    public class VisitReadDto
    {
        public Guid VisitId { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? VisitedAt { get; set; }
        public Guid PatientId { get; set; }
    }
}
