using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InterApro.Database.Tables
{
    public class RequestStatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RequestStatusId { get; set; }
        [StringLength(128)]
        public string Description { get; set; }
    }
}
