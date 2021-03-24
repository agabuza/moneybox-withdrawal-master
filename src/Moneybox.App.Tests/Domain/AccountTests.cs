using NUnit.Framework;
using System;

namespace Moneybox.App.Tests.Domain
{
    class AccountTests
    {
        [Test]
        public void Whether_Withdraw_ThrowsException_When_InsufficientFunds()
        {
            var accountguid = Guid.NewGuid();
            var account = new Account(accountguid, null, 1000, 100, 0);

            var exception = Assert.Throws<InvalidOperationException>(() => account.Transfer(100m));
            Assert.That(exception.Message, Is.EqualTo("Insufficient funds to make transfer"));
        }

        [Test]
        public void Whether_Transfer_ThrowsException_When_PayInLimitReached()
        {
            var accountguid = Guid.NewGuid();
            var account = new Account(accountguid, null, 1000, 100, 0);

            var exception = Assert.Throws<InvalidOperationException>(() => account.Transfer(100m));
            Assert.That(exception.Message, Is.EqualTo("Account pay in limit reached"));
        }
    }
}
