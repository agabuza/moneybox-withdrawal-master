using NUnit.Framework;
using System;

namespace Moneybox.App.Tests.Domain
{
    class AccountTests
    {
        [Test]
        public void Withdraw_ThrowsException_When_InsufficientFunds()
        {
            var accountguid = Guid.NewGuid();
            var account = new Account(accountguid, null, 1000, 100, 0);

            var exception = Assert.Throws<InvalidOperationException>(() => account.Withdraw(4001m));
            Assert.That(exception.Message, Is.EqualTo("Insufficient funds to make transfer"));
        }

        [Test]
        public void Transfer_ThrowsException_When_PayInLimitReached()
        {
            var accountguid = Guid.NewGuid();
            var account = new Account(accountguid, null, 1000, 100, 0);

            var exception = Assert.Throws<InvalidOperationException>(() => account.Transfer(4001m));
            Assert.That(exception.Message, Is.EqualTo("Account pay in limit reached"));
        }

        [Test]
        public void Withdraw_UpdatesWithdrawnAndBalance_When_SufficientFunds()
        {
            var accountguid = Guid.NewGuid();
            var account = new Account(accountguid, null, 1000, 100, 0);

            account.Withdraw(100);

            Assert.That(account.Withdrawn, Is.EqualTo(0));
            Assert.That(account.Balance, Is.EqualTo(900));
        }

        [Test]
        public void Transfer_UpdatesPaidInAndBalance_When_PayInLimitNotReached()
        {
            var accountguid = Guid.NewGuid();
            var account = new Account(accountguid, null, 1000, 100, 0);

            account.Transfer(100);

            Assert.That(account.PaidIn, Is.EqualTo(100));
            Assert.That(account.Balance, Is.EqualTo(1100));
        }
    }
}
