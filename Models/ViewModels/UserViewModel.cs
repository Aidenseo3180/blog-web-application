namespace Blog.Web.Models.ViewModels
{
    public class UserViewModel
    {
        //list of User
        public List<User> Users { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool AdminRoleCheck { get; set; }
    }
}
