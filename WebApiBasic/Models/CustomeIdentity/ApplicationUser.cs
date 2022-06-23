using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBasic.Models;

namespace WebApplication1.Models
{
    public class ApplicationUser:IdentityUser
    {
        public bool IsActive { get; set; }
        //Navigation Property
        public virtual IEnumerable<Visit> Visit { get; set; }

    }
}
