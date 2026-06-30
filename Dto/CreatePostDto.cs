public class CreatePostDto
{
    public int AuthorId { get; set; }

    public string? Text { get; set; }

    public IFormFile? Image { get; set; }
}