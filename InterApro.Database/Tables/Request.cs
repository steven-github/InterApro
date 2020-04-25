using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InterApro.Database.Tables
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long RequestId { get; set; }

        [StringLength(4096)]
        public string RequestDescription {get; set;}

        [Column(TypeName = "decimal(18,4)")]
        public decimal RequestAmount { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string BuyerId { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string ManagerId { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string FinanceId { get; set; }

        public long RequestStatusId { get; set; }
        public virtual RequestStatus RequestStatus { get; set; }

        public virtual IEnumerable<Log> RequestLogs { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
