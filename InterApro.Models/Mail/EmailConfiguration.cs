﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InterApro.Models.Mail
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string[] Templates { get; set; }
    }
}
