namespace NfeExplorer_Api.Application.DTOs.Responses;

public class DashboardStatsResponse
{
    public int TotalInvoices { get; set; }
    public decimal TotalAmount { get; set; }
    public int TotalOutbound { get; set; }
    public int TotalInbound { get; set; }
    public IEnumerable<TopIssuerResponse> TopIssuers { get; set; } = new List<TopIssuerResponse>();
    public IEnumerable<MonthlyInvoicesResponse> MonthlyInvoices { get; set; } = new List<MonthlyInvoicesResponse>();
}

public class TopIssuerResponse
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class MonthlyInvoicesResponse
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Amount { get; set; }
}
