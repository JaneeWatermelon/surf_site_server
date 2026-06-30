using System.ComponentModel.DataAnnotations.Schema;

public class PostDto: BaseEntityDto
{
    public int Id { get; set; }

    // public int AuthorId { get; set; }
    public UserDto Author { get; set; } = null!;

    public string? Text { get; set; }

}
