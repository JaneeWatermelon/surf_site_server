# Surf Social Network Backend

Бэкенд социальной сети для сёрферов, разработанный на **ASP.NET Core Web API**. Приложение предоставляет REST API для работы с пользователями, авторизацией, публикациями и загрузкой файлов, а также взаимодействует с базой данных через Entity Framework Core.

Реализация [фронтенда](https://github.com/JaneeWatermelon/surf_site).

Проект разработан в рамках производственной практики в компании [СМС – информационные технологии](https://sms-it.ru/).

## Стек технологий

- ASP.NET Core Web API
- C#
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Swagger (OpenAPI)

## Основной функционал

### Пользователи

- Регистрация пользователей
- Авторизация
- Получение информации о текущем пользователе
- Проверка уникальности имени пользователя и электронной почты

### Авторизация

- JWT-аутентификация
- Защита приватных маршрутов
- Проверка валидности токена

### Публикации

- Создание публикаций
- Получение списка публикаций
- Загрузка изображений
- Сортировка публикаций по дате создания (от новых к старым)

### Работа с файлами

- Загрузка изображений
- Хранение файлов на сервере
- Формирование URL для доступа к изображениям

## Архитектура проекта

```
Backend/
├── Controllers/          # Контроллеры API
├── DTO/                  # Data Transfer Objects
├── Models/               # Модели базы данных
├── Properties/
├── media/              # Загруженные файлы
├── Program.cs
└── appsettings.json
```

## Требования

- .NET 8 SDK
- PostgreSQL

## Настройка

### 1. Клонирование проекта

```bash
git clone <repository_url>
```

### 2. Настройка строки подключения

В файле `DatabaseContext.cs` укажите параметры подключения к базе данных.

Пример:

```C#
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=surf_site;Username=postgres;Password=myPassword");
}
```

## Запуск проекта

Через CLI:

```bash
dotnet run
```

После запуска API будет доступен по адресу:

```
https://localhost:5001
```

Порт `5001` может отличаться, актуальный будет указан в коммандной строке при запуске.

## Документация API

После запуска проекта документация Swagger доступна по адресу:

```
https://localhost:5001/swagger
```

## Используемые технологии

- ASP.NET Core Middleware
- Entity Framework Core
- JWT Bearer Authentication

## Структура базы данных

Основные сущности:

- User
- Post
- Другие вспомогательные сущности проекта

Связи между сущностями реализованы средствами Entity Framework Core.

## Особенности реализации

- REST API
- Архитектура Controller → Service → Database
- DTO для передачи данных
- Валидация входящих данных
- Централизованная обработка ошибок
- Entity Framework Core

## Тестирование

Проект поддерживает тестирование REST API с помощью:

- Swagger UI
- Postman
