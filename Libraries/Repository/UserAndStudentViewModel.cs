using Libraries.Model;

namespace Libraries.Repository
{
    public class UserAndStudentViewModel
    {
        public UserLoginAndSecurityTable Users { get; set; }
        public StudentTable Students { get; set; }
    }
}