using MicroFinance.Enums.Transaction;
using MicroFinance.Models.Transactions;

namespace MicroFinance.Models.Wrapper.TrasactionWrapper;


public class SubLedgerTransactionWrapper : LedgerTransactionWrapper
{
    public int SubLedgerId { get; set; }
}