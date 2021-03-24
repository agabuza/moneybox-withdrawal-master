using System;

namespace Moneybox.App
{
    public class Account
    {
        public const decimal PayInLimit = 4000m;

        public Account(
            Guid id,
            User user,
            decimal balance,
            decimal withdrawn,
            decimal paidIn)
        {
            Id = id;
            User = user;
            Balance = balance;
            Withdrawn = withdrawn;
            PaidIn = paidIn;
        }

        public Guid Id { get; private set; }

        public User User { get; private set; }

        public decimal Balance { get; private set; }

        public decimal Withdrawn { get; private set; }

        public decimal PaidIn { get; private set; }

        public void Withdraw(decimal amount)
        {
            var fromBalance = Balance - amount;
            if (fromBalance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            Withdrawn -= amount;
            Balance -= amount;
        }

        public void Transfer(decimal amount)
        {
            var paidIn = PaidIn + amount;
            if (paidIn > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            Balance += amount;
            PaidIn += amount;
        }

        public bool HasLowFunds()
        {
            return Balance < 500m;
        }

        public bool HasApproachingPayInLimit()
        {
            return Account.PayInLimit - PaidIn < 500m;
        }
    }
}
