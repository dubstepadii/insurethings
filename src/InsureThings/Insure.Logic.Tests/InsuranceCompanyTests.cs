using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Insure.Logic.Tests
{
    [TestClass]
    public class InsuranceCompanyTests
    {
        IInsuranceCompany _target;

        [TestInitialize]
        public void StartUp()
        {
            _target = new InsuranceCompany(new List<Risk>());
        }

        [TestMethod]
        public void AddRisk_NameOfInsuredObjectNull_ThrowsException()
        {
            Action result = () => _target.AddRisk(null, new Risk(), DateTime.Now, DateTime.Now);
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void AddRisk_NameOfInsuredObjectEmpty_ThrowsException()
        {
            Action result = () => _target.AddRisk(string.Empty, new Risk(), DateTime.Now, DateTime.Now);
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void AddRisk_NameOfInsuredObjectWhiteSpace_ThrowsException()
        {
            Action result = () => _target.AddRisk("", new Risk(), DateTime.Now, DateTime.Now);
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void AddRisk_ValidFromInPast_ThrowsException()
        {
            Action result = () => _target.AddRisk("object", new Risk(), DateTime.Now.AddDays(-2), DateTime.Now);
            Assert.ThrowsException<ArgumentOutOfRangeException>(result);
        }

        [TestMethod]
        public void RemoveRisk_NameOfInsuredObjectNull_ThrowsException()
        {
            Action result = () => _target.RemoveRisk(null, new Risk(), DateTime.Now, DateTime.Now);
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void RemoveRisk_NameOfInsuredObjectEmpty_ThrowsException()
        {
            Action result = () => _target.RemoveRisk(string.Empty, new Risk(), DateTime.Now, DateTime.Now);
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void RemoveRisk_NameOfInsuredObjectWhiteSpace_ThrowsException()
        {
            Action result = () => _target.RemoveRisk("", new Risk(), DateTime.Now, DateTime.Now);
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void RemoveRisk_ValidTillOutOfRange_ThrowsException()
        {
            var nameOfInsuredObject = "object";
            var policy = new Policy()
            {
                NameOfInsuredObject = nameOfInsuredObject,
                ValidFrom = DateTime.Now.AddDays(-1),
                ValidTill = DateTime.Now.AddDays(1),
            };
            this.AddFakeObjectToDb(nameOfInsuredObject, policy);

            Action result = () => _target.RemoveRisk(nameOfInsuredObject, new Risk(), DateTime.Now.AddDays(5), DateTime.Now);

            Assert.ThrowsException<ArgumentOutOfRangeException>(result);
            this.RemoveFakeObjectFromDb(nameOfInsuredObject);
        }

        [TestMethod]
        public void SellPolicy_NameOfInsuredObjectNull_ThrowsException()
        {
            Action result = () => _target.SellPolicy(null, DateTime.Now.AddDays(-2), 0, new List<Risk>());
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void SellPolicy_NameOfInsuredObjectEmpty_ThrowsException()
        {
            Action result = () => _target.SellPolicy(string.Empty, DateTime.Now.AddDays(-2), 0, new List<Risk>());
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void SellPolicy_NameOfInsuredObjectWhiteSpace_ThrowsException()
        {
            Action result = () => _target.SellPolicy("", DateTime.Now.AddDays(-2), 0, new List<Risk>());
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void SellPolicy_ValidFromInPast_ThrowsException()
        {
            Action result = () => _target.SellPolicy("object", DateTime.Now.AddDays(-2), 0, new List<Risk>());
            Assert.ThrowsException<ArgumentOutOfRangeException>(result);
        }

        [TestMethod]
        public void SellPolicy_ValidMonthsZero_ThrowsException()
        {
            Action result = () => _target.SellPolicy("object", DateTime.Now, 0, new List<Risk>());
            Assert.ThrowsException<ArgumentOutOfRangeException>(result);
        }

        [TestMethod]
        public void SellPolicy_SelectedRisksEmpty_ThrowsException()
        {
            Action result = () => _target.SellPolicy("object", DateTime.Now, 11, new List<Risk>());
            Assert.ThrowsException<ArgumentOutOfRangeException>(result);
        }

        [TestMethod]
        public void GetPolicy_NameOfInsuredObjectNull_ThrowsException()
        {
            Action result = () => _target.GetPolicy(null, DateTime.Now.AddDays(-2));
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void GetPolicy_NameOfInsuredObjectEmpty_ThrowsException()
        {
            Action result = () => _target.GetPolicy(string.Empty, DateTime.Now.AddDays(-2));
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void GetPolicy_NameOfInsuredObjectWhiteSpace_ThrowsException()
        {
            Action result = () => _target.GetPolicy("", DateTime.Now.AddDays(-2));
            Assert.ThrowsException<ArgumentNullException>(result);
        }

        [TestMethod]
        public void GetPolicy_EffectiveDateInFuture_NoPolicyFound()
        {
            var nameOfInsuredObject = "object";
            var policy = new Policy()
            {
                ValidFrom = DateTime.Now,
                ValidTill = DateTime.Now,
            };
            this.AddFakeObjectToDb(nameOfInsuredObject, policy);

            var result = _target.GetPolicy(nameOfInsuredObject, DateTime.Now.AddYears(2));

            Assert.IsNull(result);
            this.RemoveFakeObjectFromDb(nameOfInsuredObject);
        }

        [TestMethod]
        public void GetPolicy_ValidEffectiveDate_ReturnsPolicy()
        {
            var nameOfInsuredObject = "object";
            var policy = new Policy()
            {
                NameOfInsuredObject = nameOfInsuredObject,
                ValidFrom = DateTime.Now.AddDays(-1),
                ValidTill = DateTime.Now.AddDays(1),
            };
            this.AddFakeObjectToDb(nameOfInsuredObject, policy);

            var result = _target.GetPolicy(nameOfInsuredObject, DateTime.Now);

            Assert.AreEqual(policy, result);
            this.RemoveFakeObjectFromDb(nameOfInsuredObject);
        }

        [TestMethod]
        public void SellPolicy_ValidPolicy_SellsAndReturnsPolicy()
        {
            var risks = new List<Risk>()
            {
                new Risk
                {
                    Name = "abc",
                    YearlyPrice = 1000,
                }
            };
            var nameOfInsuredObject = "object";
            var policy = new Policy()
            {
                NameOfInsuredObject = nameOfInsuredObject,
                ValidFrom = DateTime.Now,
                ValidTill = DateTime.Now.AddMonths(13),
            };
            this.AddFakeObjectToDb(nameOfInsuredObject, policy);

            var result = _target.SellPolicy(nameOfInsuredObject, DateTime.Now.AddDays(1), 12, risks);

            Assert.IsNotNull(result);
            Assert.AreEqual(1000, result.Premium);

            this.RemoveFakeObjectFromDb(nameOfInsuredObject);
        }

        [TestMethod]
        public void RemoveRisk_ExistingRisk_RemovesRiskFromPolicy()
        {
            var nameOfInsuredObject = "object";
            var riskToRemove = new Risk()
            {
                Name = "Test_1",
                YearlyPrice = 1345,
            };
            var risk = new Risk()
            {
                Name = "Test",
                YearlyPrice = 1,
            };
            var selectedRisks = new List<Risk>()
            {
                risk,
                riskToRemove,
            };
            var policy = new Policy()
            {
                NameOfInsuredObject = nameOfInsuredObject,
                ValidFrom = DateTime.Now.AddMonths(-1),
                ValidTill = DateTime.Now.AddMonths(1),
                InsuredRisks = selectedRisks,
            };
            this.AddFakeObjectToDb(nameOfInsuredObject, policy);

            _target.RemoveRisk(nameOfInsuredObject, riskToRemove, DateTime.Now.AddDays(5), DateTime.Now);

            var fakePolicy = (Policy)this.GetFakeObjectFromDb(nameOfInsuredObject);
            Assert.IsTrue(fakePolicy.InsuredRisks.Count == 1);
            Assert.IsTrue(fakePolicy.InsuredRisks.All(x => x.Name == risk.Name));
            this.RemoveFakeObjectFromDb(nameOfInsuredObject);
        }

        private void AddFakeObjectToDb(string nameOfInsuredObject, IPolicy data) =>
            MemoryCache.Default.Add(nameOfInsuredObject, data, new CacheItemPolicy());

        private void RemoveFakeObjectFromDb(string nameOfInsuredObject) =>
            MemoryCache.Default.Remove(nameOfInsuredObject);

        private object GetFakeObjectFromDb(string nameOfInsuredObject) =>
                MemoryCache.Default.Get(nameOfInsuredObject);
    }
}
