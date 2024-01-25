using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Model
{
    public class UserLoginAndSecurityTable
    {
        [Key]
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int QuestionCode { get; set; }
        public string SecurityAnswer { get; set; }
        public bool Is_Active { get; set; }
    }
}
