using Api.Dtos.Dependent;
using Api.Dtos.Employee;

namespace Api.Repositories
{
    public interface IPeopleRepository
    {
        GetEmployeeDto GetEmployeeById(int id);
        List<GetEmployeeDto> GetEmployees();
        GetDependentDto GetDependentById(int id);
        List<GetDependentDto> GetDependents();

    }
}
