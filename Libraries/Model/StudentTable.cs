using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Model
{
    public class StudentTable
    {
        [Key]
        public string ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public bool Is_Active { get; set; }
        [NotMapped]
        public virtual ICollection<UserLoginAndSecurityTable> UserLoginAndSecurityList { get; set; }
    }
}

