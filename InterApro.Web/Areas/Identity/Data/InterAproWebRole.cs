using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InterApro.Web.Data
{
    public class InterAproWebRole : IdentityRole
    {
        [Column(TypeName = "decimal(18,4)")]
        public decimal? MinAmout { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal? MaxAmount { get; set; }
    }
}
