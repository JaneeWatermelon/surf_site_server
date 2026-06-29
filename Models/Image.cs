using System.ComponentModel.DataAnnotations.Schema;

public class Image: BaseEntity
{
    public int Id { get; set; }

    public string Code { get; set; }

    public virtual List<ImagePostShip> ImagePostShips { get; set; }

}