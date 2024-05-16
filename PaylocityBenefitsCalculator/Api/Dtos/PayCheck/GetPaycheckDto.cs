namespace Api.Dtos.PayCheck;

public class GetPaycheckDto
{
    public int EmployeeId { get; set; }
    public decimal BaseCost { get; set; }
    public decimal AdditionalCost { get; set; }
    public decimal TotalCost => BaseCost + AdditionalCost;
}
