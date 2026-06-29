using System.ComponentModel.DataAnnotations.Schema;

public class UserDto: BaseEntityDto
{
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

}