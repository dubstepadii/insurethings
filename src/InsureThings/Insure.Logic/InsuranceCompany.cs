using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Linq;

public class InsuranceCompany : IInsuranceCompany
{
    private MemoryCache _db = MemoryCache.Default;
    private object _dbLock = new object();
    private const string AvailableRisksKey = "AvailableRisksKey";

    public InsuranceCompany()
    {
    }

    /// <summary>
    /// We are giving the interface of Insurance company
    /// Name of Insurance company
    /// </summary>
    public string Name => "Random_Insurance";

    /// <summary>
    /// List of the risks that can be insured. List can be updated at any time
    /// </summary>
    public IList<Risk> AvailableRisks
    {
        get
        {
            return this.GetFromDb<IList<Risk>>(AvailableRisksKey);
        }

        set => throw new NotImplementedException();
    }

    /// <summary>
    /// Add risk to the policy of insured object.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of insured object</param>
    /// <param name="risk">Risk that must be added</param>
    /// <param name="validFrom">Date when risk becomes active. Can not be in the past</param>
    /// <param name="effectiveDate">Point of date and time, when the policy effective</param>
    public void AddRisk(string nameOfInsuredObject, Risk risk, DateTime validFrom, DateTime effectiveDate)
    {
        if (string.IsNullOrWhiteSpace(nameOfInsuredObject))
        {
            throw new ArgumentNullException(nameof(nameOfInsuredObject));
        }

        if (validFrom < DateTime.Now)
        {
            throw new ArgumentOutOfRangeException(nameof(validFrom));
        }



    }

    /// <summary>
    /// Gets policy with a risks at the given point of time.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of insured object</param>
    /// <param name="effectiveDate">Point of date and time, when the policy effective</param>
    /// <returns></returns>
    public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Remove risk from the policy of insured object.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of insured object</param>
    /// <param name="risk">Risk that must be removed</param>
    /// <param name="validTill">Date when risk become inactive. Must be equal to or greater than date when risk become active</param>
    /// <param name="effectiveDate">Point of date and time, when the policy effective</param>
    public void RemoveRisk(string nameOfInsuredObject, Risk risk, DateTime validTill, DateTime effectiveDate)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sell the policy.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of the insured object. Must be unique in the given period.</param>
    /// <param name="validFrom">Date and time when policy starts. Can not be in the past</param>
    /// <param name="validMonths">Policy period in months</param>
    /// <param name="selectedRisks">List of risks that must be included in the policy</param>
    /// <returns>Information about policy</returns>
    public IPolicy SellPolicy(string nameOfInsuredObject, DateTime validFrom, short validMonths, IList<Risk> selectedRisks)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets certain data from db by key
    /// </summary>
    /// <typeparam name="T">Data type which will be retrieved</typeparam>
    /// <param name="key">Key value</param>
    /// <returns></returns>
    private T GetFromDb<T>(string key)
    {
        T result = default(T);

        if (_dbLock == null)
        {
            var data = _db.Get(key);
            if (data != null)
            {
                result = (T)data;
            }
        }

        return result;
    }
}