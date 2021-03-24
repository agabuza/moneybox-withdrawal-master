using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using NUnit.Framework;
using System;

namespace Moneybox.App.Tests.Features
{
    public class WithdrawMoneyTests
    {   
        [Test]
        public void Execute_UpdatesAccount_When_SufficientFunds()
        {
            var accountguid = Guid.NewGuid();
            var account = new Account(accountguid, null, 1000, 100, 0);

            var notificationService = new Mock<INotificationService>();
            var accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(x => x.GetAccountById(accountguid)).Returns(account);

            // Act
            var withdrawMoney = new WithdrawMoney(accountRepository.Object, notificationService.Object);
            withdrawMoney.Execute(accountguid, 100);

            Assert.That(account.Withdrawn, Is.EqualTo(0));
            Assert.That(account.Balance, Is.EqualTo(900));
        }
    }
}
