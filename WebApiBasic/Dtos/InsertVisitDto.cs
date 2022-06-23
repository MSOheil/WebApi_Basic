using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBasic.Dtos
{
    public class InsertVisitDto
    {
        [Required]
        public virtual DateTime CreateAt { get; set; }
        public virtual DateTime? VisitedAt { get; set; }
        [Required]
        public virtual Guid PatientId { get; set; }
        public bool IsActive { get; set; }

    }
}
