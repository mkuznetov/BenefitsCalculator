using Api.Dtos.Employee;
using Api.Models;
using Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EmployeesController : ControllerBase
{
    IPeopleRepository _peopleRepository;

    public EmployeesController(IPeopleRepository peopleRepository)
    {
        _peopleRepository = peopleRepository ?? throw new ArgumentNullException(nameof(peopleRepository));
    }

    [SwaggerOperation(Summary = "Get employee by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetEmployeeDto>>> Get(int id)
    {
        var employee = _peopleRepository.GetEmployeeById(id); // in production, this should be an async call (await)
        return (employee != null) ?
            new ApiResponse<GetEmployeeDto>
            {
                Data = employee,
                Success = true,
            } : NotFound($"The employee {id} was not found");
    }

    [SwaggerOperation(Summary = "Get all employees")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetEmployeeDto>>>> GetAll()
    {
        var result = new ApiResponse<List<GetEmployeeDto>>
        {
            Data = _peopleRepository.GetEmployees(), // in production, this should be an async call (await)
            Success = true,
        };

        return result;
    }
}
