using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBasic.Models
{
    public class Medicine
    {
        public Guid MedicineId { get; set; }
        public string Name { get; set; }

        //Navigation Property
        public IEnumerable<VisitsMedicines> VisitsMedicines { get; set; }
    }
}
