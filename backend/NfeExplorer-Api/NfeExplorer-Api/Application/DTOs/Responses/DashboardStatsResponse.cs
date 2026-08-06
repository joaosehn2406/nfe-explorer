namespace NfeExplorer_Api.Application.DTOs.Responses;

public class DashboardStatsResponse
{
    public int TotalNotas { get; set; }
    public decimal ValorTotal { get; set; }
    public int TotalSaidas { get; set; }
    public int TotalEntradas { get; set; }
    public IEnumerable<TopEmitenteResponse> TopEmitentes { get; set; } = new List<TopEmitenteResponse>();
    public IEnumerable<NotasPorMesResponse> NotasPorMes { get; set; } = new List<NotasPorMesResponse>();
}

public class TopEmitenteResponse
{
    public string Nome { get; set; }
    public decimal Valor { get; set; }
}

public class NotasPorMesResponse
{
    public int Ano { get; set; }
    public int Mes { get; set; }
    public decimal Valor { get; set; }
}
