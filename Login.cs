using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Models
{
    public class Login
    {[Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Loginid{ get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Createdon { get; set; }
        public bool IsActive { get; set; }
    }
}
