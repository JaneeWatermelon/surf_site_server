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
    public PostDto CreatePost([FromBody] PostDto dto)
    {
        using var dataContext = new DatabaseContext();

        var post = new Post
        {
            AuthorId = dto.AuthorId,
            Text = dto.Text,
            CreationDateTime = DateTime.UtcNow,
            LastModificationDateTime = DateTime.UtcNow
        };

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