using Libraries.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Question03.Models
{
    public class UserDashboardViewModel
    {
        public UserLoginAndSecurityTable User { get; set; }
        public StudentTable Student { get; set; }
    }
}