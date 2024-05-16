using Api.Dtos.Dependent;
using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    IPeopleRepository _peopleRepository;

    public DependentsController(IPeopleRepository peopleRepository)
    {
        _peopleRepository = peopleRepository ?? throw new ArgumentNullException(nameof(peopleRepository));
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = _peopleRepository.GetDependentById(id); // in production, this should be an async call (await)
        return (dependent != null) ?
            new ApiResponse<GetDependentDto>
            {
                Data = dependent,
                Success = true,
            } : NotFound($"The dependent {id} was not found");
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = _peopleRepository.GetDependents(),  // in production, this should be an async call (await)
            Success = true,
        };

        return result;
    }
}
