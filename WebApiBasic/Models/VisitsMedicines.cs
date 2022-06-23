using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBasic.Models
{
    public class VisitsMedicines
    {
        [Key]
        public Guid VisitsMedicinesId { get; set; }
        public Guid MedicineId { get; set; }
        public Guid VisitId { get; set; }

        //Navigation Property
        [ForeignKey("MedicineId")]
        public Medicine Medicine { get; set; }
        [ForeignKey("VisitId")]
        public Visit Visit { get; set; }
    }
}
