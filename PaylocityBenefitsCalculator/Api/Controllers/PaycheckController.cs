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
    IBenefitsCalculator _benefitsCalculator;

    public PaycheckController(
        IPeopleRepository peopleRepository, IBenefitsCalculator benefitsCalculator)
    {
        _peopleRepository = peopleRepository ?? throw new ArgumentNullException(nameof(peopleRepository));
        _benefitsCalculator = benefitsCalculator ?? throw new ArgumentNullException(nameof(benefitsCalculator));
    }

    [SwaggerOperation(Summary = "Get paycheck for employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetPaycheckDto>>> GetPaycheck(int id)
    {
        var employee = _peopleRepository.GetEmployeeById(id); // in production, this should be an async call (await)
        if (employee == null)
        {
            return NotFound($"The employee {id} was not found");
        }

        // Calculate the paycheck
        var paycheck = _benefitsCalculator.CalculatePaycheckForEmployee(employee);

        var result = new ApiResponse<GetPaycheckDto>
        {
            Data = paycheck,
            Success = true,
        };

        return result;
    }
}
