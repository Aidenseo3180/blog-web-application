namespace Blog.Web.Models.ViewModels
{
    public class EditTagRequest
    {
        //good practice to have domain models and view models separately
        public Guid Id { get; set; }    //we need it from edit
        public string Name { get; set; }    //display name and display name
        public string DisplayName { get; set; }

    }
}
