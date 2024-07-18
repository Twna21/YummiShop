using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{

    public partial class Account
    {
        public int AccountID { get; set; }
        public string Photo { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public byte Type { get; set; }
        public byte Status { get; set; }
    }
}
