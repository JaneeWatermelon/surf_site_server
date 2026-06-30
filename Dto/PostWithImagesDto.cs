public class PostWithImagesDto
{
    public PostDto Post { get; set; } = null!;
    public List<ImageDto> Images { get; set; } = [];
}