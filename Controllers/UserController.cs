using Microsoft.AspNetCore.Mvc;
using System.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [Route("api/Users/Create")]
    public UserDto CreateUser([FromBody] UserDto dto)
    {
        using var dataContext = new DatabaseContext();

        var user = new User
        {
            Login = dto.Login,
            Email = dto.Email,
            Password = dto.Password,
            AvatarCode = dto.AvatarCode,
            SecondName = dto.SecondName,
            FirstName = dto.FirstName,
            ContactInfo = dto.ContactInfo,
            About = dto.About,
            Achivements = dto.Achivements,
            CreationDateTime = DateTime.UtcNow
        };

        dataContext.Users.Add(user);
        dataContext.SaveChanges();

        return new UserDto(user);
        // return new UserDto
        // {
        //     Id = user.Id,
        //     Login = user.Login,
        //     Email = user.Email,
        //     Password = user.Password,
        //     AvatarCode = user.AvatarCode,
        //     SecondName = user.SecondName,
        //     FirstName = user.FirstName,
        //     ContactInfo = user.ContactInfo,
        //     About = user.About,
        //     Achivements = user.Achivements,
        //     CreationDateTime = user.CreationDateTime,
        //     LastModificationDateTime = user.LastModificationDateTime,
        // };
    }

    [HttpGet]
    [Route("api/Users/{id}")]
    public UserDto? GetUserById(int id)
    {
        using var dataContext = new DatabaseContext();

        var user = dataContext.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDto(u))
            // .Select(u => new UserDto
            // {
            //     Id = u.Id,
            //     Login = u.Login,
            //     Email = u.Email,
            //     Password = u.Password,
            //     AvatarCode = u.AvatarCode,
            //     SecondName = u.SecondName,
            //     FirstName = u.FirstName,
            //     ContactInfo = u.ContactInfo,
            //     About = u.About,
            //     Achivements = u.Achivements,
            //     CreationDateTime = u.CreationDateTime,
            //     LastModificationDateTime = u.LastModificationDateTime,
            // })
            .FirstOrDefault();

        if (user == null)
            throw new KeyNotFoundException($"Пользователь с id = {id} не найден");

        return user;
    }

    [HttpPost]
    [Route("api/Users/Register")]
    public async Task<ActionResult<UserDto>> Register([FromForm] RegisterDto dto)
    {
        using var dataContext = new DatabaseContext();
        var errors = new Dictionary<string, string[]>();

        if (dataContext.Users.Any(u => u.Login == dto.Login))
        {
            // throw new Exception("Пользователь с таким псевдонимом уже существует.");
            errors["login"] =
            [
                "Такой псевдоним уже занят."
            ];
            return ValidationProblem(new ValidationProblemDetails
            {
                Errors = errors
            });
        }

        if (dataContext.Users.Any(u => u.Email == dto.Email))
        {
            // throw new Exception("Пользователь с такой почтой уже существует.");
            errors["email"] =
            [
                "Такая почта уже зарегистрирована."
            ];
            return ValidationProblem(new ValidationProblemDetails
            {
                Errors = errors
            });
        }

        if (dto.Password != dto.PasswordRepeat)
        {
            // throw new Exception("Пароли не совпадают.");
            errors["passwordRepeat"] =
            [
                "Пароли не совпадают."
            ];
            return ValidationProblem(new ValidationProblemDetails
            {
                Errors = errors
            });
        }

        var user = new User
        {
            Login = dto.Login,
            Email = dto.Email,
            Password = dto.Password,
            // AvatarCode = dto.AvatarCode,
            SecondName = dto.SecondName,
            FirstName = dto.FirstName,
            ContactInfo = dto.ContactInfo,
            About = dto.About,
            Achivements = dto.Achivements,
            CreationDateTime = DateTime.UtcNow
        };

        if (dto.Avatar != null)
        {
            var images_dir = "media/images/users";
            var fileExt = Path.GetExtension(dto.Avatar.FileName);
            var fileName = Guid.NewGuid() + fileExt;

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            var extension = fileExt.ToLowerInvariant();

            if (!allowed.Contains(extension))
            {
                // return BadRequest("Недопустимый формат файла.");
                errors["avatar"] =
                [
                    "Недопустимый формат файла."
                ];
                return ValidationProblem(new ValidationProblemDetails
                {
                    Errors = errors
                });
            }
            
            var path = Path.Combine(images_dir, fileName);

            Directory.CreateDirectory(images_dir);

            await using var stream = System.IO.File.Create(path);
            await dto.Avatar.CopyToAsync(stream);

            user.AvatarCode = images_dir + "/" + fileName;
        } 
        else
        {
            user.AvatarCode = "";
        }

        if (errors.Count > 0)
        {
            return ValidationProblem(new ValidationProblemDetails
            {
                Errors = errors
            });
        }

        dataContext.Users.Add(user);
        dataContext.SaveChanges();

        return GetUserById(user.Id)!;
    }

    [HttpPost]
    [Route("api/Users/Login")]
    public ActionResult<LoginOutDto> Login([FromBody] LoginDto dto)
    {
        using var dataContext = new DatabaseContext();
        var errors = new Dictionary<string, string[]>();

        var user = dataContext.Users.FirstOrDefault(u => (
            u.Login == dto.LoginOrEmail ||
            u.Email == dto.LoginOrEmail
        ));

        if (user == null)
        {
            // throw new Exception("Пользователь не найден.");
            errors["loginOrEmail"] =
            [
                "Пользователя с такими данными не существует."
            ];
            return ValidationProblem(new ValidationProblemDetails
            {
                Errors = errors
            });
        }

        if (user.Password != dto.Password)
        {
            // throw new Exception("Неверный пароль.");
            errors["password"] =
            [
                "Неверный пароль."
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

        var token = GenerateJwtToken(user);
    
        // формируем ответ
        var response = new LoginOutDto
        {
            Access_token = token,
            User = GetUserById(user.Id)!
        };
    
        return response;

        // return GetUserById(user.Id)!;
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}