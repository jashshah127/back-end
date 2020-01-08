using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Request
{
    public class Log_in
    {
        public string Password { get; set; }
        public string Username { get; set; }
        
    }
}

