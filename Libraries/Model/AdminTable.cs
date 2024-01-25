using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Model
{
    public class AdminTable
    {
        [Key]
        public int AdminID { get; set; }
        public string AdminName { get; set; }
        public string Password { get; set; }
        public bool Is_Active { get; set; }
    }
}
