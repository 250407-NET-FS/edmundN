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

app.MapPost("/user/{username}/history", async (string username, AddToHistoryRequest request, IHistoryService historyService) =>
{
    try
    {
        historyService.AddToHistory(username, request.VideoUrl, request.Title);
        return Results.Ok("History entry added successfully.");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/addvideo", async (AddVideoRequest request, IVideoRepo videoRepo) =>
{
    try
    {
        var video = new Video
        {
            Url = request.Url,
            Title = request.Title
        };

        videoRepo.addvideo(video);
        return Results.Created($"/video/{video.Url}", video);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});
//====================================================================================================
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

app.MapGet("/user/{username}/history", async (string username, IUserRepo userRepo, IHistoryService historyService) =>
{
    try
    {
        var user = userRepo.GetUser(username);
        if (user == null)
        {
            return Results.NotFound($"User '{username}' not found");
        }

        var history = historyService.GetUserHistory(username);

        var response = new
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


// =============================================================================
app.Run();
