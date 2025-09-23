using FIAPCloudGames.API.Extensions;
using FIAPCloudGames.API.Jwt;
using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Domain.Enumerators;
using FIAPCloudGames.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FIAPCloudGames.API.Endpoints;

public static class UserEndpoints
{

    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users");

        group.MapGet("/", async (IUserService service, [FromQuery] int page = 1, [FromQuery] int pageSize = 10) => {

            if (page <= 0)
                page = 1;

            if (pageSize <= 0)
                pageSize = 1;

            if (pageSize > 100)
                pageSize = 100;

            int skip = (page - 1) * pageSize;

            var users = await service.FindAll(skip: skip, take: pageSize);

            return Results.Ok(users?.Select(item => new GetUserResponse { Id = item.Id, Name = item.Name, Role = item.Role.GetDescription() }));
        }).RequireAuthorization("AdminOnly");

        group.MapGet("/{id:guid}", async (IUserService service, [FromRoute] Guid id) => {
            var user = await service.Find(id: id);

            if (user == null)
                return Results.NotFound();

            return Results.Ok(new GetUserResponse { Id = user.Id, Name = user.Name, Role = user.Role.GetDescription() });
        }).RequireAuthorization("AdminOnly");

        group.MapDelete("/{id:guid}", async (IUserService service, [FromRoute] Guid id) => {
            var user = await service.Find(id: id);

            if (user == null)
                return Results.NotFound();

            await service.Delete(id: user.Id);

            return Results.NoContent();
        }).RequireAuthorization("AdminOnly");

        group.MapPost("/", async (IUserService service, [FromBody] CreateUserRequest request) => {
            try
            {
                if (request == null)
                    throw new InvalidOperationException("Invalid body");

                Guid id = await service.Create(new Domain.Entities.User(
                    name: request.Name,
                    email: request.Email,
                    password: request.Password,
                    role: request.Role
                ));

                return Results.Ok(new GetUserResponse { Id = id, Name = request.Name, Role = request.Role.GetDescription() });
            }
            catch (Exception ex) {
                return Results.BadRequest(new GenericMessageResponse { Message = ex.Message });
            }

        }).RequireAuthorization("AdminOnly");
        
        group.MapPatch("/{id:guid}", async (IUserService service, [FromRoute] Guid id, [FromBody] UpdateUserRequest request, HttpContext httpContext) => {
            if (request == null)
                return Results.BadRequest(new GenericMessageResponse { Message = "Invalid body" });

            var userIdFromToken = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleFromToken = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userIdFromToken, out var loggedUserId))
                return Results.Unauthorized();

            // Se o usuário logado for Customer (0) e estiver tentando editar outro usuário, nega
            if (roleFromToken == UserRole.Customer.ToString() && id != loggedUserId)
                return Results.Forbid();

            var userFound = await service.Find(id: id);
            if (userFound == null)
                return Results.BadRequest(new GenericMessageResponse { Message = "User not found!" });

            userFound.Update(
                name: request.Name,
                email: request.Email,
                role: request.Role
            );

            await service.Update(userFound);

            return Results.Ok();
        }).RequireAuthorization();

        group.MapPatch("/update-password/{id:guid}", async (IUserService service, [FromRoute] Guid id, [FromBody] UpdateUserPasswordRequest request, HttpContext httpContext) => {
            if (request == null)
                return Results.BadRequest(new GenericMessageResponse { Message = "Invalid body" });

            var userIdFromToken = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleFromToken = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (!Guid.TryParse(userIdFromToken, out var loggedUserId))
                return Results.Unauthorized();

            // Se o usuário logado for Customer (0) e estiver tentando editar outro usuário, nega
            if (roleFromToken == UserRole.Customer.ToString() && id != loggedUserId)
                return Results.Forbid();

            var userFound = await service.Find(id: id);
            if (userFound == null)
                return Results.BadRequest(new GenericMessageResponse { Message = "User not found!" });

            userFound.UpdatePassword(newPassword: request.Password);

            await service.Update(userFound);

            return Results.Ok();
        }).RequireAuthorization();

        group.MapPost("/login", async (IConfiguration config, IUserService service, [FromBody] UserLoginRequest request) => {
            if (request == null)
                return Results.BadRequest(new GenericMessageResponse { Message = "Invalid body" });

            var userFound = await service.Login(email: request.Email, password: request.Password);
            if (userFound == null)
                return Results.BadRequest(new GenericMessageResponse { Message = "Wrong email or password!" });

            string token = JwtGenerator.Generate(user: userFound, config: config);

            return Results.Ok(new UserLoginResponse { Id = userFound.Id, Token = token });
        }).AllowAnonymous();

        group.MapPost("/register", async (IUserService service, [FromBody] CreateUserRequest request) => {
            if (request == null)
                return Results.BadRequest(new GenericMessageResponse { Message = "Invalid body" });

            var userFound = await service.FindByEmail(email: request.Email);
            if (userFound != null)
                return Results.BadRequest(new GenericMessageResponse { Message = "Invalid email!" });

            Guid id = await service.Create(new Domain.Entities.User(
                name: request.Name,
                email: request.Email,
                password: request.Password,
                role: Domain.Enumerators.UserRole.Customer
            ));

            return Results.Created();
        }).AllowAnonymous();

        return app;
    }
}
