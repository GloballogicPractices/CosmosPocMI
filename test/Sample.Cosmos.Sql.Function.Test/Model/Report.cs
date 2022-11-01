using System;

namespace Sample.Cosmos.Sql.Function.Test.Model;

public class Report
{
    /// <summary>
    /// Gets or sets  The unique identifier for reports
    /// </summary>
    /// <example>12345</example>
    public int ReportId { get; set; }

    /// <summary>
    /// Gets or sets  Unique identifier of the printer
    /// </summary>
    /// <example>12345</example>
    public string PrinterId { get; set; }

    /// <summary>
    /// Gets or sets  report name
    /// </summary>
    /// <example>dummyName</example>
    public string ReportName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether flag indicates whether to print landscape or not
    /// </summary>
    /// <example>true</example>
    public bool PrintLandscape { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether flag indicates whether to Generated ExcelReportWhenEmailed true or not
    /// </summary>
    /// <example>false</example>
    public bool GenerateExcelReportWhenEmailed { get; set; }

    /// <summary>
    /// Gets or sets  Audit column will be used to track the create history of records
    /// </summary>
    /// <example>1</example>
    public long CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets Audit column will be used to track the create history date-time of records
    /// </summary>
    /// <example>2021-07-25T18:30:00Z</example>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets Audit column will be used to track the create history of records
    /// </summary>
    /// <example>1</example>
    public long? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets Audit column will be used to track the create history date-time of record
    /// </summary>
    /// <example>2021-07-25T18:30:00Z</example>
    public DateTime? UpdatedOnUtc { get; set; }
}
