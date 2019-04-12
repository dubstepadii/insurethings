using System;
using System.Collections.Generic;

public class Policy : IPolicy
{
    /// <summary>
    /// Name of insured object
    /// </summary>
    public string NameOfInsuredObject { get; set; }

    /// <summary>
    /// Date when policy becomes active
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// Date when policy becomes inactive
    /// </summary>
    public DateTime ValidTill { get; set; }

    /// <summary>
    /// Total price of the policy. Calculate by summing up all insured risks.
    /// Take into account that risk price is given for 1 full year. Policy/risk period can be shorter.
    /// </summary>
    public decimal Premium { get; set; }

    /// <summary>
    /// Initially included risks of risks at specific moment of time.
    /// </summary>
    public IList<Risk> InsuredRisks { get; set; }
}