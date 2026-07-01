using Microsoft.AspNetCore.Mvc;

[ApiController]
public class PostController: ControllerBase
{
    [HttpGet]
    [Route("api/Posts/Get")]
    public List<PostWithImagesDto> GetPosts()
    {
        using var dataContext = new DatabaseContext();

        return dataContext.Posts
            .OrderByDescending(p => p.CreationDateTime)
            .ThenByDescending(p => p.Id)
            .Select(p => new PostWithImagesDto
            {
                Post = new PostDto
                {
                    Id = p.Id,
                    Author = new UserDto
                    {
                        Id = p.Author.Id,
                        Login = p.Author.Login,
                        Email = p.Author.Email,
                        Password = p.Author.Password,
                        AvatarCode = p.Author.AvatarCode,
                        SecondName = p.Author.SecondName,
                        FirstName = p.Author.FirstName,
                        ContactInfo = p.Author.ContactInfo,
                        About = p.Author.About,
                        Achivements = p.Author.Achivements,
                        CreationDateTime = p.Author.CreationDateTime,
                        LastModificationDateTime = p.Author.LastModificationDateTime
                    },
                    Text = p.Text,
                    CreationDateTime = p.CreationDateTime,
                    LastModificationDateTime = p.LastModificationDateTime
                },
                Images = p.ImagePostShips
                    .Select(s => new ImageDto
                    {
                        Id = s.Image.Id,
                        Code = s.Image.Code,
                        CreationDateTime = s.Image.CreationDateTime,
                        LastModificationDateTime = s.Image.LastModificationDateTime
                    })
                    .ToList()
            })
            .ToList();
    }

    [HttpPost]
    [Route("api/Posts/Create")]
    public async Task<ActionResult<PostWithImagesDto>> CreatePost([FromForm] CreatePostDto dto)
    {
        using var dataContext = new DatabaseContext();
        var errors = new Dictionary<string, string[]>();

        var user = dataContext.Users.FirstOrDefault(u => u.Id == dto.AuthorId);

        if (user == null)
        {
            // throw new Exception("Пользователь не найден.");
            errors[""] =
            [
                "Пользователь не найден."
            ];
            return ValidationProblem(new ValidationProblemDetails
            {
                Errors = errors
            });
        }

        if (string.IsNullOrWhiteSpace(dto.Text) &&
            dto.Image == null)
        {
            // throw new Exception("Пост должен содержать текст или фотографию.");
            errors[""] =
            [
                "Пост должен содержать текст или фотографию."
            ];
            return ValidationProblem(new ValidationProblemDetails
            {
                Errors = errors
            });
        }

        if (errors.Count > 0)
        {
            return ValidationProblem(new ValidationProblemDetails
            {
                Errors = errors
            });
        }

        var nowTime = DateTime.UtcNow;

        var post = new Post
        {
            Author = user,
            Text = dto.Text,
            CreationDateTime = nowTime,
            LastModificationDateTime = nowTime
        };

        Image? image = null;

        if (dto.Image != null) {
            var images_dir = "media/images/posts";
            var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var path = Path.Combine(images_dir, fileName);

            Directory.CreateDirectory(images_dir);

            await using var stream = System.IO.File.Create(path);
            await dto.Image.CopyToAsync(stream);

            var imageCode = images_dir + "/" + fileName;

            image = new Image
            {
                Code = imageCode,
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

        var result = new PostWithImagesDto
        {
            Post = new PostDto {
                Id = post.Id,
                Author = new UserDto
                {
                    Id = post.Author.Id,
                    Login = post.Author.Login,
                    Email = post.Author.Email,
                    Password = post.Author.Password,
                    AvatarCode = post.Author.AvatarCode,
                    SecondName = post.Author.SecondName,
                    FirstName = post.Author.FirstName,
                    ContactInfo = post.Author.ContactInfo,
                    About = post.Author.About,
                    Achivements = post.Author.Achivements,
                    CreationDateTime = post.Author.CreationDateTime,
                    LastModificationDateTime = post.Author.LastModificationDateTime
                },
                Text = post.Text,
                CreationDateTime = post.CreationDateTime,
                LastModificationDateTime = post.LastModificationDateTime
            },
            Images = [],
        };

        if (image != null)
        {
            result.Images.Add(
                new ImageDto {
                    Id = image.Id,
                    Code = image.Code,
                    CreationDateTime = image.CreationDateTime,
                    LastModificationDateTime = image.LastModificationDateTime
                }
            );
        }

        return result;
    }
}