
namespace School_View_Models
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastMessage { get; set; }
        public string avatar { get; set; } = "assets/user.png";
        public bool IsGroup { get; set; }
    }
}
