using System.ComponentModel.DataAnnotations.Schema;

public class ImagePostShipDto
{
    public int Id { get; set; }

    public int PostId { get; set; }
    
    public int ImageId { get; set; }


}