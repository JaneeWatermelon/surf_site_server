using System.ComponentModel.DataAnnotations.Schema;

public class ImageDto: BaseEntityDto
{
    public int Id { get; set; }

    public string Code { get; set; }

}