using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Insure.Logic.Tests
{
    [TestClass]
    public class InsuranceCompanyTests
    {
        IInsuranceCompany _target;

        [TestInitialize]
        public void StartUp()
        {
            _target = new InsuranceCompany();
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
            Action result = () => _target.RemoveRisk("object", new Risk(), DateTime.Now.AddDays(5), DateTime.Now);
            Assert.ThrowsException<ArgumentOutOfRangeException>(result);
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
    }
}
