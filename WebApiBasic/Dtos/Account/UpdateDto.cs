using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBasic.Dtos.Account
{
    public class UpdateDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Remote(action: "VeryfyEmail", controller: "Account")]
        public string Email { get; set; }
    }
}
