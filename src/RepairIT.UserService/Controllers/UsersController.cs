using Microsoft.AspNetCore.Mvc;
using RepairIT.Shared.Users;
using RepairIT.UserService.Services;

namespace RepairIT.UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService, ILogger<UsersController> logger)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        logger.LogInformation("Getting all users");
        var result = await userService.GetAllAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<User>> GetById(Guid id)
    {
        logger.LogInformation("Getting user with ID {Id}", id);
        var result = await userService.GetByIdAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error == UserErrors.CouldNotFindUserById)
        {
            return NotFound();
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    [HttpPost]
    public async Task<ActionResult<User>> Add([FromBody] User user)
    {
        logger.LogInformation("Adding a new user with ID {Id}", user.Id);
        var result = await userService.AddAsync(user);

        if (result.IsSuccess)
        {
            var userCreated = result.Value;
            return CreatedAtAction(nameof(GetById), new { id = userCreated.Id }, userCreated);
        }

        if (result.Error == UserErrors.EmailAlreadyUsed)
        {
            return Conflict(result.Error.Message);
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }
}
