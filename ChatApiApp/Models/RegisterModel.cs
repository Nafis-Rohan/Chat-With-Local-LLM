using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatApiApp.Models
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}