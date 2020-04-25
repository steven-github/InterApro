using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterApro.Database.Tables
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogId { get; set;  }

        [StringLength(4096)]
        public string LogDescription { get; set; }
        public DateTime LogDate { get; set; }

        public long? RequestStatusId { get; set; }
        public virtual RequestStatus RequestStatus { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string UserId { get; set; }

    }
}
