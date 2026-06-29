using System.ComponentModel.DataAnnotations.Schema;

public class ImagePostShip
{
    public int Id { get; set; }

    public int PostId { get; set; }
    
    [ForeignKey(nameof(PostId))]
    public virtual Post Post { get; set; }
    public int ImageId { get; set; }

    [ForeignKey(nameof(ImageId))]
    public virtual Image Image { get; set; }

}