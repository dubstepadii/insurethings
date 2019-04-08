using System;
using System.Collections.Generic;

public class InsuranceCompany : IInsuranceCompany
{
    public InsuranceCompany()
    {
    }

    public string Name => throw new NotImplementedException();

    public IList<Risk> AvailableRisks { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom, DateTime effectiveDate)
    {
        throw new NotImplementedException();
    }

    public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
    {
        throw new NotImplementedException();
    }

    public void RemoveRisk(string nameOfInsuredObject, Risk risk, DateTime validTill, DateTime effectiveDate)
    {
        throw new NotImplementedException();
    }

    public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
    {
        throw new NotImplementedException();
    }
}