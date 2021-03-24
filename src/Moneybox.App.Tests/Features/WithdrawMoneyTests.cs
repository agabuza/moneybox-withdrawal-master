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
        private Guid accountguid;
        private Account account;
        private Mock<INotificationService> notificationService;
        private Mock<IAccountRepository> accountRepository;

        [SetUp]
        public void Setup()
        {
            this.accountguid = Guid.NewGuid();
            this.account = new Account(accountguid, null, 1000, 100, 0);
            this.notificationService = new Mock<INotificationService>();
            this.accountRepository = new Mock<IAccountRepository>();
            accountRepository.Setup(x => x.GetAccountById(accountguid)).Returns(account);
        }

        [Test]
        public void Execute_UpdatesWithdrawnAndBalance_When_SufficientFunds()
        {
            var withdrawMoney = new WithdrawMoney(accountRepository.Object, notificationService.Object);
            withdrawMoney.Execute(accountguid, 100);

            Assert.That(account.Withdrawn, Is.EqualTo(0));
            Assert.That(account.Balance, Is.EqualTo(900));
        }

        [Test]
        public void Execute_UpdatesAccountInRepo_When_SufficientFunds()
        {
            var withdrawMoney = new WithdrawMoney(accountRepository.Object, notificationService.Object);
            withdrawMoney.Execute(accountguid, 100);

            accountRepository.Verify(x => x.Update(account), Times.AtLeast(1));
        }

        [Test]
        public void Execute_DoesNotUpdateAccountInRepo_When_InsufficientFunds()
        {
            var withdrawMoney = new WithdrawMoney(accountRepository.Object, notificationService.Object);
            
            var exception = Assert.Throws<InvalidOperationException>(() => withdrawMoney.Execute(accountguid, 4001m));
            accountRepository.Verify(x => x.Update(account), Times.Never);
        }
    }
}
