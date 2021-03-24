using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using NUnit.Framework;
using System;

namespace Moneybox.App.Tests.Features
{
    class TransferMoneyTests
    {
        private Mock<INotificationService> notificationService;
        private Mock<IAccountRepository> accountRepository;
        private Guid sourceAccountGuid;
        private Account sourceAccount;
        private Guid targetAccountGuid;
        private Account targetAccount;

        [SetUp]
        public void Setup()
        {
            sourceAccountGuid = Guid.NewGuid();
            sourceAccount = new Account(sourceAccountGuid, null, 1000, 100, 100);

            targetAccountGuid = Guid.NewGuid();
            targetAccount = new Account(targetAccountGuid, null, 1000, 100, 100);

            notificationService = new Mock<INotificationService>();
            accountRepository = new Mock<IAccountRepository>();

            accountRepository
                .Setup(x => x.GetAccountById(sourceAccountGuid))
                .Returns(sourceAccount);

            accountRepository
                .Setup(x => x.GetAccountById(targetAccountGuid))
                .Returns(targetAccount);
        }

        [Test]
        public void Execute_UpdatesSourceAccount_When_Success()
        {
            // Act
            var transferMoneyFeature = new TransferMoney(accountRepository.Object, notificationService.Object);
            transferMoneyFeature.Execute(sourceAccountGuid, targetAccountGuid, 100);

            Assert.That(sourceAccount.Withdrawn, Is.EqualTo(0));
            Assert.That(sourceAccount.Balance, Is.EqualTo(900));
        }

        [Test]
        public void Execute_UpdatesTargetAccount_When_Success()
        {
            // Act
            var transferMoneyFeature = new TransferMoney(accountRepository.Object, notificationService.Object);
            transferMoneyFeature.Execute(sourceAccountGuid, targetAccountGuid, 100);

            Assert.That(targetAccount.Balance, Is.EqualTo(1100));
            Assert.That(targetAccount.PaidIn, Is.EqualTo(200));
        }

        [Test]
        public void Execute_DoesNotUpdateAccountsInRepo_When_InsufficientFunds()
        {
            // Act
            var withdrawMoney = new WithdrawMoney(accountRepository.Object, notificationService.Object);

            var exception = Assert.Throws<InvalidOperationException>(() => withdrawMoney.Execute(sourceAccountGuid, 4001m));
            accountRepository.Verify(x => x.Update(sourceAccount), Times.Never);
            accountRepository.Verify(x => x.Update(targetAccount), Times.Never);
        }
    }
}
