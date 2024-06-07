namespace MicroFinance.Dtos;

public class VoucherDto
{
    public decimal TransactionAmount { get; set; }
    public string AmountInWords { get; set; }
    public string? CollectedBy { get; set; }
    public string? ModeOfPayment { get; set; }
    public string? VoucherNumber { get; set; }
    public string? TransactionDateBS { get; set; }
    public string? RealWorldTransactionDateBS { get; set; }
    public DateTime? TransactionDateAD { get; set; }
    public DateTime? RealWorldTransactionDateAD { get; set; }

}