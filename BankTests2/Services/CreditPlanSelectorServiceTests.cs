using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfPaging.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPaging.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using WpfPaging.Pages;
using System.Numerics;
using Newtonsoft.Json.Bson;

namespace WpfPaging.Services.Tests
{
    [TestClass()]
    public class CreditPlanSelectorServiceTests
    {
        private BankContext _context;

        [TestInitialize]
        public void Setup()
        {
            _context = GetInMemoryDb();
            Console.WriteLine("Setup executed.");
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            Console.WriteLine("Cleanup executed.");
        }

        [TestMethod()]
        public async Task PointsBoundaryValuesTest()
        {
            User user = GetDefaultUser();
            var creditTargetAuto = Models.Enums.CreditTarget.Auto;

            var selectService = new CreditPlanSelectorService(_context);

            // Проверка пограничных значений очков к кредитному плану плану 
            var plans = await selectService.SelectAvailableCreditPlans(user, creditTargetAuto);
            Assert.AreEqual(2, plans.Count);
            Assert.IsTrue(plans.Select(x => x.Id).Contains(1));
            Assert.IsTrue(plans.Select(x => x.Id).Contains(2));
            Assert.IsTrue(!plans.Select(x => x.Id).Contains(3));
        }

        [TestMethod()]
        public async Task CreditTargetTest()
        {
            User user = GetDefaultUser();
            var selectService = new CreditPlanSelectorService(_context);
            var creditTargetOther = Models.Enums.CreditTarget.Other;

            // Проверка цели кредита 
            var plans = await selectService.SelectAvailableCreditPlans(user, creditTargetOther);
            Assert.AreEqual(1, plans.Count);
            Assert.AreEqual(creditTargetOther, plans.First().Target);

            _context.Database.EnsureDeleted();
        }

        [TestMethod()]
        public async Task PointsPriorityTest()
        {
            User user = GetDefaultUser();
            user.Age = 21;
            var creditTargetOther = Models.Enums.CreditTarget.Other;
            var selectService = new CreditPlanSelectorService(_context);

            // Проверка что при пересечении элементов рейтинга выбирается наибольший результат  
            var plans = await selectService.SelectAvailableCreditPlans(user, creditTargetOther);
            Assert.AreEqual(2, plans.Count);
            Assert.AreEqual(creditTargetOther, plans.First().Target);
            Assert.AreEqual(202, plans.First().Amount);
        }

        [TestMethod()]
        public async Task PointsSummTest()
        {
            User user = new User()
            {
                Age = 18,
                Qualification = Models.Enums.QualificationTypes.None,
                JobType = Models.Enums.JobTypes.PublicService,
                FamilyState = Models.Enums.FamilyStates.Single,
                KidsAmount = 1,
            };
            var creditTargetMortgage = Models.Enums.CreditTarget.Mortgage;
            var selectService = new CreditPlanSelectorService(_context);

            // Проверка что учитываются все типы элементов рейтинга  
            var plans = await selectService.SelectAvailableCreditPlans(user, creditTargetMortgage);
            Assert.AreEqual(1, plans.Count);
            Assert.AreEqual(creditTargetMortgage, plans.First().Target);
            Assert.AreEqual(202, plans.First().Amount);
            Assert.AreEqual(24, plans.First().MinimalScore);
        }

        private BankContext GetInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<BankContext>()
                .UseInMemoryDatabase(databaseName: "Bank")
                .Options;

            var context = new BankContext(options);
            var bank = new Bank()
            {
                ScoringSystemsItems = new List<ScoringSystemItem>()
                {
                    new ScoringSystemItem() { Type = Models.Enums.ScoringItemType.Age, MinCondition = 18, MaxCondition = 25, Points = 20 },
                    new ScoringSystemItem() { Type = Models.Enums.ScoringItemType.Age, MinCondition = 21, MaxCondition = 25, Points = 25 },
                    new ScoringSystemItem() { Type = Models.Enums.ScoringItemType.KidsAmount, MinCondition = 1, MaxCondition = 2, Points = 1 },
                    new ScoringSystemItem() { Type = Models.Enums.ScoringItemType.JobType, ExactValue = 0, Points = 1 },
                    new ScoringSystemItem() { Type = Models.Enums.ScoringItemType.FamilyState, ExactValue = 0, Points = 1 },
                    new ScoringSystemItem() { Type = Models.Enums.ScoringItemType.Qualification, ExactValue = 0, Points = 1 },
                },
                CreditPlans = new List<CreditPlan>()
                {
                    new CreditPlan() { Amount = 100, MinimalScore = 19, Target = Models.Enums.CreditTarget.Auto },
                    new CreditPlan() { Amount = 101, MinimalScore = 20, Target = Models.Enums.CreditTarget.Auto },
                    new CreditPlan() { Amount = 102, MinimalScore = 21, Target = Models.Enums.CreditTarget.Auto },
                    new CreditPlan() { Amount = 102, MinimalScore = 20, Target = Models.Enums.CreditTarget.Other },
                    new CreditPlan() { Amount = 202, MinimalScore = 25, Target = Models.Enums.CreditTarget.Other },
                    new CreditPlan() { Amount = 202, MinimalScore = 24, Target = Models.Enums.CreditTarget.Mortgage },
                    new CreditPlan() { Amount = 202, MinimalScore = 25, Target = Models.Enums.CreditTarget.Mortgage },
                }
            };
            context.Banks.Add(bank);
            context.SaveChanges();
            return context;
        }

        private User GetDefaultUser()
        {
            return new User()
            {
                Age = 18,
                FamilyState = Models.Enums.FamilyStates.Married,
                JobType = Models.Enums.JobTypes.CommercialStructure,
                KidsAmount = 0,
                Qualification = Models.Enums.QualificationTypes.Specialist
            };
        }
    }
}