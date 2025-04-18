using modals;
using repository;
using services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHistoryRepo, JsonHistoryRepo>();
builder.Services.AddSingleton<IUserRepo, JsonUserRepo>();
builder.Services.AddSingleton<IVideoRepo, JsonVideoRepo>();

builder.Services.AddSingleton<IHistoryService, HistoryService>();

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "proj1";
    config.Title = "proj v1";
    config.Version = "v1";
});

var app = builder.Build();

//swagger
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "proj1";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}
;
//=========================================================================================================================//

app.MapPost("/adduser", (AddUserRequest request, IUserRepo repo) =>
{
    try
    {
        if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Email))
        {
            return Results.BadRequest("Username and email are required");
        }

        if (repo.GetUser(request.Username) != null)
        {
            return Results.BadRequest("Username already exists");
        }

        var user = new Users
        {
            Id = Guid.NewGuid(),
            Username = request.Username,
            Email = request.Email
        };

        repo.AddUser(user);
        return Results.Created($"/user/{user.Username}", user);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});



app.MapPost("/user/{username}/history", (string username, AddToHistoryRequest request, IHistoryService historyService, IVideoRepo videoRepo) =>
{
    try
    {
        var video = videoRepo.GetById(request.VideoId);
        if (video == null)
        {
            return Results.NotFound($"Video with ID '{request.VideoId}' not found");
        }

        historyService.AddToHistory(username, request.VideoId);
        return Results.Ok(new
        {
            Message = "History entry added successfully.",
            Username = username,
            VideoId = request.VideoId,
            VideoTitle = video.Title
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/getuser/{username}", (string username, IUserRepo repo) =>
{
    try
    {
        var user = repo.GetUser(username);
        if (user == null)
        {
            return Results.NotFound($"User '{username}' not found");
        }
        return Results.Ok(user);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});


app.MapGet("/user/{username}/history", (string username, IUserRepo userRepo, IHistoryService historyService) =>
{
    try
    {
        var user = userRepo.GetUser(username);
        if (user == null)
        {
            return Results.NotFound($"User '{username}' not found");
        }

        var history = historyService.GetUserHistory(username);
        var response = new UserHistoryResponse
        {
            User = user,
            VideoHistory = history.OrderByDescending(h => h.WatchedAt)
        };

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/users", (IUserRepo userRepo) =>
{
    try
    {
        var users = userRepo.LoadUsers();
        return Results.Ok(users);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});


app.MapGet("/videos", (IVideoRepo videoRepo) =>
{
    try
    {
        var videos = videoRepo.GetAll();
        return Results.Ok(videos);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});


app.MapGet("/video/{videoId}", (Guid videoId, IVideoRepo videoRepo, IHistoryRepo historyRepo) =>
{
    try
    {
        var video = videoRepo.GetById(videoId);
        if (video == null)
        {
            return Results.NotFound($"Video with ID '{videoId}' not found");
        }
        var allHistory = historyRepo.GetAllHistory();
        var usersWhoAdded = allHistory
        .Where(h => h.VideoId == videoId)
        .Select(h => h.Username)
        .Distinct()
        .ToList();

        var response = new
        {
            Video = video,
            UsersWhoAdded = usersWhoAdded
        };

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/addvideo", (AddVideoRequest request, IVideoRepo videoRepo) =>
{
    try
    {
        var video = new Video
        {
            Id = Guid.NewGuid(),
            Url = request.Url,
            Title = request.Title
        };

        videoRepo.addvideo(video);
        return Results.Created($"/video/{video.Id}", video);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapDelete("/user/{username}", (string username, IUserRepo repo) =>
{
    try
    {
        var user = repo.GetUser(username);
        if (user == null)
        {
            return Results.NotFound($"User  '{username}' not found");
        }

        repo.DeleteUser(username);
        return Results.Ok($"User  '{username}' has been deleted successfully.");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapGet("/video/{videoId}/users", (Guid videoId, IHistoryRepo historyRepo, IUserRepo userRepo) =>
{
    try
    {
        // Get all history entries for this video
        var allHistory = historyRepo.GetAllHistory();
        var historyEntries = allHistory.Where(h => h.VideoId == videoId).ToList();

        if (!historyEntries.Any())
        {
            return Results.NotFound($"No users found who have watched video with ID '{videoId}'");
        }
        //unique usernames
        var usernames = historyEntries.Select(h => h.Username).Distinct().ToList();

        var users = usernames
            .Select(username => userRepo.GetUser(username))
            .Where(user => user != null)
            .ToList();

        var response = users.Select(user => new
        {
            User = user,
            WatchHistory = historyEntries
                .Where(h => h.Username == user.Username)
                .OrderByDescending(h => h.WatchedAt)
                .Select(h => new
                {
                    h.WatchedAt,
                    h.AddedAt
                })
                .ToList()
        }).ToList();

        return Results.Ok(new
        {
            VideoId = videoId,
            UserCount = users.Count,
            Users = response
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();
