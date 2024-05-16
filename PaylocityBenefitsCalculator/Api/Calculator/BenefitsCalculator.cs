using Api.Dtos.Employee;
using Api.Dtos.PayCheck;

public class BenefitsCalculator: IBenefitsCalculator
{
    public GetPaycheckDto CalculatePaycheckForEmployee(GetEmployeeDto employee)
    {
        // Note: the employee has been already validated for the business requirement
        // "not more than 1 spouse or domestic partner" (PeopleRepository)

        var paycheck = new GetPaycheckDto()
        {
            EmployeeId = employee.Id,
            Salary = 1m * employee.Salary / 26, // annual salary recalculated to 2 weeks
            BaseCost = 1m * 1000 * 12 / 26, // monthly base cost recalculated to 2 weeks
            HighSalaryTwoPercentDeduction = employee.Salary > 80000 ? (0.02m * employee.Salary / 26) : 0m,
        };
        foreach (var dependent in employee.Dependents)
        {
            int dependentAge = DateTime.Today.Year - dependent.DateOfBirth.Year; // maybe adjust "Today" to the specific timezone (UTC)
            if (dependent.DateOfBirth > DateTime.Today.AddYears(-dependentAge)) dependentAge--;

            var monthlyDependentCost = dependentAge > 50 ? (600 + 200) : 600; // monthly dependent cost
            var dependentCost = 1m * monthlyDependentCost * 12 / 26; // monthly dependent cost recalculated to 2 weeks

            paycheck.DependentCosts.Add(dependentCost);
        }

        return paycheck;
    }
}
