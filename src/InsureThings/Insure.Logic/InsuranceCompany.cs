using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

public class InsuranceCompany : IInsuranceCompany
{
    private MemoryCache _db = MemoryCache.Default;
    private readonly object _dbLock = new object();

    public InsuranceCompany(IList<Risk> availableRisks)
    {
        AvailableRisks = availableRisks;
    }

    /// <summary>
    /// We are giving the interface of Insurance company
    /// Name of Insurance company
    /// </summary>
    public string Name => "Random_Insurance";

    /// <summary>
    /// List of the risks that can be insured. List can be updated at any time
    /// </summary>
    public IList<Risk> AvailableRisks { get; set; }

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

        var policy = this.GetPolicy(nameOfInsuredObject, effectiveDate);
        if (policy == null)
        {
            throw new NullReferenceException($"No valid policy found by name {nameOfInsuredObject}");
        }

        policy.InsuredRisks.Add(risk);
        this.WriteToDb(nameOfInsuredObject, policy);
    }

    /// <summary>
    /// Gets policy with a risks at the given point of time.
    /// </summary>
    /// <param name="nameOfInsuredObject">Name of insured object</param>
    /// <param name="effectiveDate">Point of date and time, when the policy effective</param>
    /// <returns></returns>
    public IPolicy GetPolicy(string nameOfInsuredObject, DateTime effectiveDate)
    {
        var policy = this.ReadFromDb<IPolicy>(nameOfInsuredObject);
        return policy.ValidFrom <= effectiveDate && policy.ValidTill >= effectiveDate ? policy : null;
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
        if (string.IsNullOrWhiteSpace(nameOfInsuredObject))
        {
            throw new ArgumentNullException(nameof(nameOfInsuredObject));
        }

        var policy = this.GetPolicy(nameOfInsuredObject, effectiveDate);
        if (policy == null)
        {
            throw new NullReferenceException($"Valid policy not found by name ${nameOfInsuredObject}");
        }

        if (validTill >= policy.ValidFrom)
        {
            throw new ArgumentOutOfRangeException(nameof(validTill));
        }

        policy.InsuredRisks.Remove(risk);
        this.WriteToDb<IPolicy>(nameOfInsuredObject, policy);
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
        if (string.IsNullOrWhiteSpace(nameOfInsuredObject))
        {
            throw new ArgumentNullException(nameof(nameOfInsuredObject));
        }

        if (validFrom < DateTime.Now)
        {
            throw new ArgumentOutOfRangeException(nameof(validFrom));
        }

        if (validMonths == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(validMonths));
        }

        if (!selectedRisks.Any())
        {
            throw new ArgumentOutOfRangeException(nameof(selectedRisks));
        }

        var policy = this.GetPolicy(nameOfInsuredObject, validFrom.AddMonths(validMonths));
        if (policy == null)
        {
            throw new NullReferenceException($"No valid policy found by name {nameOfInsuredObject}");
        }

        decimal allRiskYearPrices = 0;
        selectedRisks.Select(x => allRiskYearPrices += x.YearlyPrice);
        var premium = allRiskYearPrices * (validMonths / 12);

        policy.ValidFrom = validFrom;
        policy.ValidTill = validFrom.AddMonths(validMonths);
        policy.Premium = premium;

        return policy;
    }

    /// <summary>
    /// Gets certain data from db by key
    /// </summary>
    /// <typeparam name="T">Data type which will be retrieved</typeparam>
    /// <param name="key">Key value</param>
    /// <returns></returns>
    private T ReadFromDb<T>(string key)
    {
        T result = default(T);

        if (_dbLock == null)
        {
            lock (_dbLock)
            {
                var data = _db.Get(key);
                if (data != null)
                {
                    result = (T)data;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Write data to DB
    /// </summary>
    /// <typeparam name="T">Data type which will be saved</typeparam>
    /// <param name="key">Key for object to save</param>
    /// <param name="data">Data object</param>
    private void WriteToDb<T>(string key, T data)
    {
        var existingData = _db.Get(key);
        if (data == null)
        {
            if (_dbLock == null)
            {
                lock (_dbLock)
                {
                    existingData = _db.Get(key);
                    if (data != null)
                    {
                        _db.Remove(key);
                    }

                    _db.Add(key, data, new CacheItemPolicy());
                }
            }
        }
    }
}