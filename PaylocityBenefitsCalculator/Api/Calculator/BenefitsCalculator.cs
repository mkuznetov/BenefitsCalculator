using Api.Calculator;
using Api.Dtos.Employee;
using Api.Dtos.PayCheck;

public class BenefitsCalculator: IBenefitsCalculator
{
    readonly int MonthesInYear = 12;
    readonly int TwoWeekSpansInYear = 26;

    public GetPaycheckDto CalculatePaycheckForEmployee(GetEmployeeDto employee)
    {
        // Note: the employee has been already validated for the business requirement
        // "not more than 1 spouse or domestic partner" (PeopleRepository)

        var dependentsTotalCost = 0m;
        foreach (var dependent in employee.Dependents)
        {
            var monthlyDependentCost = dependent.DateOfBirth.OlderThan(50) ? (600 + 200) : 600; // monthly dependent cost
            var dependentCost = 1m * monthlyDependentCost * MonthesInYear / TwoWeekSpansInYear; // monthly dependent cost recalculated to 2 weeks
            dependentsTotalCost += dependentCost;
        }

        var paycheck = new GetPaycheckDto()
        {
            EmployeeId = employee.Id,
            Salary = 1m * employee.Salary / TwoWeekSpansInYear, // annual salary recalculated to 2 weeks
            BaseCost = 1m * 1000 * MonthesInYear / TwoWeekSpansInYear, // monthly base cost recalculated to 2 weeks
            DependentsCost = dependentsTotalCost,
            HighSalaryTwoPercentDeduction = employee.Salary > 80000 ? (0.02m * employee.Salary / TwoWeekSpansInYear) : 0m,
        };

        return paycheck;
    }
}
