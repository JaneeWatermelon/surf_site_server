using Microsoft.AspNetCore.Mvc;

[ApiController]
public class PostController: ControllerBase
{
    [HttpGet]
    [Route("api/Posts/Get")]
    public List<PostDto> GetPosts()
    {
        using var dataContext = new DatabaseContext();
        return dataContext.Posts
        .Select(p => new PostDto { 
            Id = p.Id, 
            AuthorId = p.AuthorId,
            Text = p.Text,
            CreationDateTime = p.CreationDateTime,
            LastModificationDateTime = p.LastModificationDateTime,
        })
        .ToList();
    }

    [HttpPost]
    [Route("api/Posts/Create")]
    public PostDto CreatePost([FromBody] CreatePostDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Text) &&
            string.IsNullOrWhiteSpace(dto.Photo))
        {
            throw new Exception("Пост должен содержать текст или фотографию.");
        }

        using var dataContext = new DatabaseContext();

        var user = dataContext.Users.FirstOrDefault(u => u.Id == dto.AuthorId);

        if (user == null)
            throw new Exception("Пользователь не найден.");

        var nowTime = DateTime.UtcNow;

        var post = new Post
        {
            Author = user,
            Text = dto.Text,
            CreationDateTime = nowTime,
            LastModificationDateTime = nowTime
        };
        if (!string.IsNullOrWhiteSpace(dto.Photo)) {
            var image = new Image
            {
                Code = dto.Photo,
                CreationDateTime = nowTime,
                LastModificationDateTime = nowTime
            };
            var imagePostShip = new ImagePostShip
            {
                Post = post,
                Image = image
            };
            
            dataContext.Images.Add(image);
            dataContext.ImagePostShips.Add(imagePostShip);
        }

        dataContext.Posts.Add(post);
        dataContext.SaveChanges();

        return new PostDto
        {
            Id = post.Id,
            AuthorId = post.AuthorId,
            Text = post.Text,
            CreationDateTime = post.CreationDateTime,
            LastModificationDateTime = post.LastModificationDateTime
        };
    }
}