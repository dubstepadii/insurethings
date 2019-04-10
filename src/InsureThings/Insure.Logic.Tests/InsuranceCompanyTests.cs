using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
    }
}
