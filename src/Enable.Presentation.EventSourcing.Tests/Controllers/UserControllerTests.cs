using Enable.Presentation.EventSourcing.Api.Layer.Controllers;
using Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Requests;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Enable.Presentation.EventSourcing.Tests;

public class UsersControllerTests
{
    private readonly UsersController _controller;
    private readonly Mock<IMediator> _mediatorMock = new();

    public UsersControllerTests()
    {
        var services = new ServiceCollection();
        services.AddSingleton(_mediatorMock.Object);
        var serviceProvider = services.BuildServiceProvider();
        _controller = new UsersController(serviceProvider.GetRequiredService<IMediator>());
    }

    [Fact]
    public async Task Read_ValidUserId_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, FirstName = "Test", LastName = "User", Email = "test@test.com", PhoneNumber = "555-555-5555" };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUser>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.Read(userId, CancellationToken.None) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(user, result.Value);
    }

    [Fact]
    public async Task Update_ValidUserId_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, FirstName = "Test", LastName = "User", Email = "test@test.com", PhoneNumber = "555-555-5555" };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateUser>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.Update(userId, CancellationToken.None) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(user, result.Value);
    }

    [Fact]
    public async Task Add_ValidUser_ReturnsOk()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, FirstName = "Test", LastName = "User", Email = "test@test.com", PhoneNumber = "555-555-5555" };
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AddUser>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var content = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user)));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext 
            {  
                Request = 
                { 
                    ContentLength = content.Length,
                    ContentType = "application/json",
                    Body = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user)))  
                } 
            }
        };

        // Act
        var result = await _controller.Add(CancellationToken.None) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        Assert.Equal(user, result.Value);
    }

    [Fact]
    public async Task Add_ValidUser_ReturnsNull()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                Request =
                {
                    ContentLength = 0,
                }
            }
        };

        // Act
        var result = await _controller.Add(CancellationToken.None) as OkObjectResult;

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Delete_ValidUserId_ReturnsOk()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteUser>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);


        // Act
        var result = await _controller.Delete(userId, CancellationToken.None) as OkResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
    }
}
