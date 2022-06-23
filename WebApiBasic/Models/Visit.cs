using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApiBasic.Models
{
    public class Visit
    {
        [Key]
        public Guid VisitId { get; set; }
        public Guid PatientId { get; set; }
        [Required]
        public DateTime CreateAt { get; set; }
        public DateTime? VisitedAt { get; set; }
        public bool IsActive { get; set; }
        public string UserId { get; set; }
        public string DoctorId { get; set; }

        //Navigation Property
        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }
        public IEnumerable<VisitsMedicines> VisitsMedicines { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser ApplicationUser { get; set; }     
        [ForeignKey("DoctorId")]
        public ApplicationUser ApplicationUserDoctorIde { get; set; }

    }
}
