using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InterApro.Web.Data
{
    public class InterAproWebUser : IdentityUser
    {
        [StringLength(128)]
        public string FirstName { get; set; }

        [StringLength(128)]
        public string LastName { get; set; }

        public bool Status { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual InterAproWebUser Manager { get; set; }
    }
}
