using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Application.Exception;
using NfeExplorer_Api.Application.Interfaces;
using NfeExplorer_Api.Application.Mappings;
using NfeExplorer_Api.Application.Validators;
using NfeExplorer_Api.Domain.Entities;
using NfeExplorer_Api.Domain.Enums;
using NfeExplorer_Api.Domain.Interfaces;
using NfeExplorer_Api.Infrastructure.Parsers;

namespace NfeExplorer_Api.Application.Services;

public class NotaFiscalService : INotaFiscalService
{
    private readonly INotaFiscalRepository _repository;

    public NotaFiscalService(INotaFiscalRepository repository)
    {
        _repository = repository;
    }

    public async Task<ImportNfeResponse> AddAsync(ParseNfeRequest request)
    {
        var nomeArquivo = request?.File?.FileName ?? "XML colado";
        NotaFiscal? notaFiscal = null;

        try
        {
            NFeValidator.ValidarRequest(request);

            string xml;
            if (request!.File != null)
            {
                using var stream = request.File.OpenReadStream();
                using var reader = new StreamReader(stream);
                xml = await reader.ReadToEndAsync();
            }
            else
            {
                xml = request.XmlText!;
            }

            NFeValidator.ValidarXml(xml);

            notaFiscal = NfeParser.Parse(xml);

            if (notaFiscal.ChaveAcesso.Length > 44)
                throw new ArgumentException("Chave de nota ultrapassa 44 dígitos.");

            var notaExistente = await _repository.GetByChaveAsync(notaFiscal.ChaveAcesso);
            if (notaExistente != null)
                throw new DuplicataNfeException($"Nota #{notaFiscal.NumeroNota} já importada (chave duplicada).");

            await _repository.AddAsync(notaFiscal);
            await _repository.AddImportLogAsync(ImportLogMapper.Sucesso(nomeArquivo, notaFiscal));

            return ImportNfeMapper.ToImportNfeResponse(notaFiscal);
        }
        catch (DuplicataNfeException ex)
        {
            await _repository.AddImportLogAsync(ImportLogMapper.Duplicada(nomeArquivo, notaFiscal, ex.Message));
            throw;
        }
        catch (ArgumentException ex)
        {
            await _repository.AddImportLogAsync(ImportLogMapper.Erro(nomeArquivo, ex.Message));
            throw;
        }
    }

    public async Task<NfeDetailsResponse?> GetByIdAsync(Guid id)
    {
        var notaFiscal = await _repository.GetByIdAsync(id);
        return notaFiscal == null ? null : NfeDetailsMapper.ToResponse(notaFiscal);
    }

    public async Task<NfeDetailsResponse?> GetByChaveAsync(string chave)
    {
        var notaFiscal = await _repository.GetByChaveAsync(chave);
        return notaFiscal == null ? null : NfeDetailsMapper.ToResponse(notaFiscal);
    }

    public Task<bool> DeleteAsync(Guid id) => _repository.DeleteAsync(id);

    public async Task<PagedResponse<NfeListItemResponse>> GetAllAsync(NfeListRequest filter)
    {
        var (items, total) = await _repository.GetAllAsync(filter);

        return new PagedResponse<NfeListItemResponse>
        {
            Items = items.Select(NfeListMapper.ToListItem).ToList(),
            Total = total,
            Page = filter.Page < 1 ? 1 : filter.Page,
            PageSize = filter.PageSize < 1 ? 10 : filter.PageSize
        };
    }

    public Task<IReadOnlyList<string>> GetEmitentesAsync() => _repository.GetEmitentesAsync();

    public async Task<DashboardStatsResponse> GetDashboardAsync()
    {
        var topEmitentes = await _repository.GetTopEmitentesAsync(5);
        var porMes = await _repository.GetValorPorMesAsync();

        return new DashboardStatsResponse
        {
            TotalNotas = await _repository.CountAsync(),
            ValorTotal = await _repository.SumValorTotalAsync(),
            TotalSaidas = await _repository.CountByTipoAsync(TipoNota.Saida),
            TotalEntradas = await _repository.CountByTipoAsync(TipoNota.Entrada),
            TopEmitentes = topEmitentes
                .Select(item => new TopEmitenteResponse { Nome = item.Nome, Valor = item.Valor })
                .ToList(),
            NotasPorMes = porMes
                .Select(item => new NotasPorMesResponse { Ano = item.Ano, Mes = item.Mes, Valor = item.Valor })
                .ToList()
        };
    }

    public async Task<IReadOnlyList<ImportLogResponse>> GetHistoricoAsync(StatusImportacao? status)
    {
        var logs = await _repository.GetImportLogsAsync(status);
        return logs.Select(ImportLogMapper.ToResponse).ToList();
    }
}
