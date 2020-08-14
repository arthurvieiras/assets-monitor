using System;
using System.Collections.Generic;
using System.Text;

namespace AssetsMonitor.Domain.Messager
{
    class EmailConfiguration
    {
        public string Domain { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string FromEmail { get; set; }
        public List<string> ToEmailList { get; set; }
    }
}
