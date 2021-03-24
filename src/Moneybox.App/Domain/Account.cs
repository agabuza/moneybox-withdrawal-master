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
            Withdrawn -= amount;
            Balance -= amount;
        }

        public void Transfer(decimal amount)
        {

            Balance += amount;
            PaidIn += amount;
        }
    }
}
