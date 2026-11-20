using Microsoft.AspNetCore.Mvc;
using RepairIT.CatalogService.Services;
using RepairIT.Shared.Computers;

namespace RepairIT.CatalogService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComputersController(
    IComputerService computerService,
    ILogger<ComputersController> logger
) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Computer>>> GetAll()
    {
        logger.LogInformation("Getting all computers");
        var result = await computerService.GetAllAsync();

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Computer>> GetById(Guid id)
    {
        logger.LogInformation("Getting computer with ID {Id}", id);
        var result = await computerService.GetByIdAsync(id);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error == ComputerErrors.CouldNotFindComputerById)
        {
            return NotFound();
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    [HttpPost]
    public async Task<ActionResult<Computer>> Add([FromBody] Computer computer)
    {
        logger.LogInformation("Adding a new computer with ID {Id}", computer.Id);
        var result = await computerService.AddAsync(computer);

        if (result.IsSuccess)
        {
            var computerCreated = result.Value;
            return CreatedAtAction(
                nameof(GetById),
                new { id = computerCreated.Id },
                computerCreated
            );
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Computer>> Update(Guid id, [FromBody] Computer computer)
    {
        if (id != computer.Id)
        {
            return BadRequest();
        }

        logger.LogInformation("Updating computer with ID {Id}", id);
        var result = await computerService.UpdateAsync(id, computer);

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        if (result.Error == ComputerErrors.CouldNotFindComputerById)
        {
            return NotFound();
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        logger.LogInformation("Deleting computer with ID {Id}", id);
        var result = await computerService.DeleteAsync(id);

        if (result.IsSuccess)
        {
            return NoContent();
        }

        if (result.Error == ComputerErrors.CouldNotFindComputerById)
        {
            return NotFound();
        }

        return Problem(statusCode: StatusCodes.Status500InternalServerError);
    }
}
