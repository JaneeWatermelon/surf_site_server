using System.ComponentModel.DataAnnotations.Schema;

public class User: BaseEntity
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string Login { get; set; }

    [Column(TypeName = "varchar(31)")]
    public string Email { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string Password { get; set; }

    public string? AvatarCode { get; set; }

    [Column(TypeName = "varchar(31)")]
    public string? SecondName { get; set; }

    [Column(TypeName = "varchar(31)")]
    public string? FirstName { get; set; }

    [Column(TypeName = "varchar(255)")]
    public string? ContactInfo { get; set; }

    [Column(TypeName = "varchar(255)")]
    public string? About { get; set; }

    [Column(TypeName = "varchar(255)")]
    public string? Achivements { get; set; }


    public virtual List<Post> Posts { get; set; }
}