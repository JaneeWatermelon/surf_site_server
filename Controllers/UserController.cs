using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        return new UserDto
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email,
            Password = user.Password,
            AvatarCode = user.AvatarCode,
            SecondName = user.SecondName,
            FirstName = user.FirstName,
            ContactInfo = user.ContactInfo,
            About = user.About,
            Achivements = user.Achivements,
            CreationDateTime = user.CreationDateTime,
            LastModificationDateTime = user.LastModificationDateTime,
        };
    }

    [HttpGet]
    [Route("api/Users/{id}")]
    public UserDto? GetUserById(int id)
    {
        using var dataContext = new DatabaseContext();

        var user = dataContext.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Login = u.Login,
                Email = u.Email,
                Password = u.Password,
                AvatarCode = u.AvatarCode,
                SecondName = u.SecondName,
                FirstName = u.FirstName,
                ContactInfo = u.ContactInfo,
                About = u.About,
                Achivements = u.Achivements,
                CreationDateTime = u.CreationDateTime,
                LastModificationDateTime = u.LastModificationDateTime,
            })
            .FirstOrDefault();

        if (user == null)
            throw new KeyNotFoundException($"Пользователь с id = {id} не найден");

        return user;
    }

    [HttpPost]
    [Route("api/Users/Register")]
    public UserDto Register([FromBody] RegisterDto dto)
    {
        using var dataContext = new DatabaseContext();

        if (dataContext.Users.Any(u => u.Login == dto.Login))
            throw new Exception("Пользователь с таким псевдонимом уже существует.");

        if (dataContext.Users.Any(u => u.Email == dto.Email))
            throw new Exception("Пользователь с такой почтой уже существует.");

        if (dto.Password != dto.PasswordRepeat)
            throw new Exception("Пароли не совпадают.");

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

        return GetUserById(user.Id)!;
    }

    [HttpPost]
    [Route("api/Users/Login")]
    public UserDto Login([FromBody] LoginDto dto)
    {
        using var dataContext = new DatabaseContext();

        var user = dataContext.Users.FirstOrDefault(u => u.Login == dto.Login);

        if (user == null)
            throw new Exception("Пользователь не найден.");

        if (user.Password != dto.Password)
            throw new Exception("Неверный пароль.");

        return GetUserById(user.Id)!;
    }
}