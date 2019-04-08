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
        public void AddRisk_NameOfInsuredObectNull_ThrowsException()
        {
            Action result = () => _target.AddRisk(null, new Risk(), DateTime.Now, DateTime.Now);
            Assert.ThrowsException<ArgumentNullException>(result);
        }
    }
}
