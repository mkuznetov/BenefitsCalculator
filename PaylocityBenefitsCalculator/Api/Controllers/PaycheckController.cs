using Api.Dtos.PayCheck;
using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaycheckController : ControllerBase
{
    IPeopleRepository _peopleRepository;

    public PaycheckController(IPeopleRepository peopleRepository)
    {
        _peopleRepository = peopleRepository ?? throw new ArgumentNullException(nameof(peopleRepository));
    }

    [SwaggerOperation(Summary = "Get paycheck for employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> GetPaycheck(int id)
    {
        var employee = _peopleRepository.GetDependentById(id); // in production, this should be an async call (await)
        if (employee == null)
        {
            return NotFound($"The employee {id} was not found");
        }
        var paycheck = new GetPaycheckDto();

        // TODO: calculate the paycheck

        var result = new ApiResponse<GetPaycheckDto>
        {
            Data = paycheck,
            Success = true,
        };

        return result;
    }
}
