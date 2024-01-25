using Libraries.Model;
using Libraries.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Question03.Models
{
    public class AdminDashboardViewModel
    {
        public List<UserAndStudentViewModel> UserAndStudentList { get; set; }
    }
}