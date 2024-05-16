using Api.Dtos.Employee;
using Api.Dtos.PayCheck;

public interface IBenefitsCalculator
{
    GetPaycheckDto CalculatePaycheckForEmployee(GetEmployeeDto employee);
}
