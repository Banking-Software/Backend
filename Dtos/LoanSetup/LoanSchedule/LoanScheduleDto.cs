namespace MicroFinance.Dtos.LoanSetup;

public class LoanScheduleDto
{
    public string PaymentDateBS { get; set; }
    public DateTime PaymentDateAD { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal InterestAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
}

public class LoanScheduleDtos
{
    public List<LoanScheduleDto> LoanScheduleDto { get; set; }
    public decimal TotalInterestAmount { get; set; }
    public decimal TotalPrincipalAmount { get; set; }
}