using System.ComponentModel.DataAnnotations.Schema;

public class Post: BaseEntity
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public virtual User Author { get; set; }

    [Column(TypeName = "varchar(31)")]
    public string Text { get; set; }

    public virtual List<ImagePostShip> ImagePostShips { get; set; }

}