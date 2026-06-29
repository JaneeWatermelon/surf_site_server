using System.ComponentModel.DataAnnotations.Schema;

public class RegisterDto: BaseEntityDto
{

    public string Login { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }
    public string PasswordRepeat { get; set; }

    public IFormFile? Avatar { get; set; }

    public string? SecondName { get; set; }

    public string? FirstName { get; set; }

    public string? ContactInfo { get; set; }

    public string? About { get; set; }

    public string? Achivements { get; set; }

}