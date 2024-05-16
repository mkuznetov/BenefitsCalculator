namespace Api.Dtos.PayCheck;

// A paycheck for the 2 weeks
// 1 year = 12 month = 52 weeks => 52/2 == 26 paychecks
public class GetPaycheckDto
{
    // Id of the employee for the paycheck
    public int EmployeeId { get; set; }

    // The Salary corresponding to 2 weeks
    public decimal Salary { get; set; }

    // Assuming the "benefits" are actually "deductions"

    // Base deduction (as per 2 weeks)
    public decimal BaseCost { get; set; }

    // (optional) Cumulative deduction for all the dependents (as per 2 weeks)
    public decimal DependentsCost { get; set; }

    // (optional) Deduction for high salary (as per 2 weeks)
    public decimal HighSalaryTwoPercentDeduction { get; set; }

    // The total cost must stay positive after appying all the deductions
    public decimal Total => Salary - BaseCost - DependentsCost - HighSalaryTwoPercentDeduction;
}
