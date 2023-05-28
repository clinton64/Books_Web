using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { set; get; }
        public string? StreetAddress { set; get; }
        public string? City { set; get;}
        public string? State { set; get; }
        public string? PostalCode { set; get;}
        public int? CompanyId { get; set;}
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company Company { get; set;}
    }
}
