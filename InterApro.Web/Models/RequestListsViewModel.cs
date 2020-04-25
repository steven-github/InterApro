using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterApro.Database.Tables;

namespace InterApro.Web.Models
{
    public class RequestListsViewModel
    {
        public List<Request> NewRequests { get; set; }
        public List<Request> ApprovedRequests { get; set; }
        public List<Request> RejectedRequests { get; set; }
    }
}
