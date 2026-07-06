using System.ComponentModel.DataAnnotations.Schema;

public class UserDto: BaseEntityDto
{

    public UserDto()
    {
        Id = 0;
        Login = "";
        Email = "";
        Password = "";
    }

    public UserDto(User user)
    {
        Id = user.Id;
        Login = user.Login;
        Email = user.Email;
        Password = user.Password;
        AvatarCode = user.AvatarCode;
        SecondName = user.SecondName;
        FirstName = user.FirstName;
        ContactInfo = user.ContactInfo;
        About = user.About;
        Achivements = user.Achivements;
        Posts = user.Posts;
    }
    public int Id { get; set; }

    public string Login { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string? AvatarCode { get; set; }

    public string? SecondName { get; set; }

    public string? FirstName { get; set; }

    public string? ContactInfo { get; set; }

    public string? About { get; set; }

    public string? Achivements { get; set; }

    public virtual List<Post>? Posts { get; set; }

}
