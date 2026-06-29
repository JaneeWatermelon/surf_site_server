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
            Text = p.Text,
            CreationDateTime = p.CreationDateTime,
            LastModificationDateTime = p.LastModificationDateTime,
        })
        .ToList();
    }
}