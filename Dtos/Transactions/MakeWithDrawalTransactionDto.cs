using System.ComponentModel.DataAnnotations;
using MicroFinance.Enums.Transaction;

namespace MicroFinance.Dtos.Transactions
{
    public class MakeWithDrawalTransactionDto : BaseDepositAccountTransactionDto    
    {
        [Required]
        public WithDrawalTypeEnum WithDrawalType { get; set; }
        public int WithDrawalChequeNumber  { get; set; }
    }
}