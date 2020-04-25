using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InterApro.Models
{
    public class User
    {
        public long UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public int Status { get; set; } = 1;
    }
}
