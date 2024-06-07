namespace MicroFinance.Helpers
{
    public class LockManager
    {
        private readonly Dictionary<int, SemaphoreSlim> accountLocks = new Dictionary<int, SemaphoreSlim>();
        private readonly Dictionary<int, SemaphoreSlim> shareAccountLocks = new Dictionary<int, SemaphoreSlim>();
        private readonly Dictionary<int, SemaphoreSlim> shareKittaLocks = new Dictionary<int, SemaphoreSlim>();
        private readonly Dictionary<int, SemaphoreSlim> ledgerLocks = new Dictionary<int, SemaphoreSlim>();
        private readonly Dictionary<int, SemaphoreSlim> subLedgerLocks = new Dictionary<int, SemaphoreSlim>();
        private readonly object accountGlobalLock = new object();
        private readonly object shareAccountGlobalLock = new object();
        private readonly object shareKittaGlobalLock = new object();
        private readonly object ledgerGlobalLock = new object();
        private readonly object subLedgerGlobalLock = new object();
        private static SemaphoreSlim _calendarMutex = new SemaphoreSlim(1, 1);
        private static readonly LockManager instance = new LockManager();
        public static LockManager Instance => instance;

        private LockManager()
        {

        }
        public SemaphoreSlim GetAccountLock(int accountId)
        {
            lock (accountGlobalLock)
            {
                if (!accountLocks.TryGetValue(accountId, out var accountLock))
                {
                    accountLock = new SemaphoreSlim(1,1);
                    accountLocks[accountId] = accountLock;
                }
                return accountLock;
            }
        }
        public SemaphoreSlim GetShareAccountLock(int shareAccountId)
        {
            lock (shareAccountGlobalLock)
            {
                if (!shareAccountLocks.TryGetValue(shareAccountId, out var shareAccountLock))
                {
                    shareAccountLock = new SemaphoreSlim(1,1);
                    shareAccountLocks[shareAccountId] = shareAccountLock;
                }
                return shareAccountLock;
            }
        }
        public SemaphoreSlim GetShareKittaLock(int shareKittaId)
        {
            lock (shareKittaGlobalLock)
            {
                if (!shareKittaLocks.TryGetValue(shareKittaId, out var shareKittaLock))
                {
                    shareKittaLock = new SemaphoreSlim(1,1);
                    shareKittaLocks[shareKittaId] = shareKittaLock;
                }
                return shareKittaLock;
            }
        }
        public SemaphoreSlim GetLedgerLock(int ledgerId)
        {
            lock (ledgerGlobalLock)
            {
                if (!ledgerLocks.TryGetValue(ledgerId, out var ledgerLock))
                {
                    ledgerLock = new SemaphoreSlim(1,1);
                    ledgerLocks[ledgerId] = ledgerLock;
                }
                return ledgerLock;
            }
        }
        public SemaphoreSlim GetSubLedgerLock(int subLedgerId)
        {
            lock (subLedgerGlobalLock)
            {
                if (!accountLocks.TryGetValue(subLedgerId, out var subLedgerLock))
                {
                    subLedgerLock = new SemaphoreSlim(1,1);
                    subLedgerLocks[subLedgerId] = subLedgerLock;
                }
                return subLedgerLock;
            }
        }

        public static async Task<IDisposable> LockCalendarAsync()
        {
            await _calendarMutex.WaitAsync();
            return new LockReleaser();
        }

        private class LockReleaser : IDisposable
        {
            public void Dispose()
            {
                _calendarMutex.Release();
            }
        }
    }
}