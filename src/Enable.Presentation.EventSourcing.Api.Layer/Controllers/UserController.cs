using Enable.Presentation.EventSourcing.Business.Layer.Features.Users.Mediatr.Commands;
using Enable.Presentation.EventSourcing.Infrastructure.Layer.Data.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enable.Presentation.EventSourcing.Api.Layer.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> Read(Guid id, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new GetUser
        {
            UserId = id
        }, cancellationToken)
        switch
        {
            null => NotFound(),
            var user => Ok(user)
        };
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> Add(CancellationToken cancellationToken = default)
    {
        if (!Request.ContentLength.HasValue || Request.ContentLength == 0)
        {
            return BadRequest();
        }

        var user = await Request.ReadFromJsonAsync<User>(cancellationToken: cancellationToken);
        if (user == null)
        {
            return BadRequest();
        }

        return await _mediator.Send(new AddUser
        {
            User = user
        }, cancellationToken)
        switch
        {
            null => NotFound(),
            var newUser => Ok(newUser)
        };

    }

    [HttpPut("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new UpdateUser
        {
            UserId = id
        }, cancellationToken)
        switch
        {
            null => NotFound(),
            var user => Ok(user)
        };
    }

    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
    {
        return await _mediator.Send(new DeleteUser
        {
            UserId = id
        }, cancellationToken)
        switch
        {
            false => NotFound(),
            true => Ok()
        };
    }

}
