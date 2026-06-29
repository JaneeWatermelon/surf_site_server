using System.ComponentModel.DataAnnotations.Schema;

public class PostDto: BaseEntityDto
{
    public int Id { get; set; }

    public string Text { get; set; }

}
