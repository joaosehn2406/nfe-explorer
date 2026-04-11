using NfeExplorer_Api.Application.DTOs.Requests;
using NfeExplorer_Api.Application.DTOs.Responses;
using NfeExplorer_Api.Application.Interfaces;
using NfeExplorer_Api.Application.Mappings;
using NfeExplorer_Api.Application.Validators;
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
        NFeValidator.ValidarRequest(request);

        string xml;

        if (request.File != null)
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

        var notaFiscal = NfeParser.Parse(xml);

        if (notaFiscal.ChaveAcesso.Length > 44)
            throw new ArgumentException("Chave de nota ultrapassa 44 dígitos.");

        var notaExistente = await _repository.GetByChaveAsync(notaFiscal.ChaveAcesso);
        if (notaExistente != null)
            throw new ArgumentException("Nota #" + notaExistente.ChaveAcesso + " já importada (chave duplicada).");

        await _repository.AddAsync(notaFiscal);

        return ImportNfeMapper.ToImportNfeResponse(notaFiscal);
    }

    public async Task<NfeDetailsResponse?> GetByIdAsync(Guid id)
    {
        var notaFiscal = await _repository.GetByIdAsync(id);

        if (notaFiscal == null)
            return null;

        return NfeDetailsMapper.ToResponse(notaFiscal);
    }

    public async Task<NfeDetailsResponse?> GetByChaveAsync(string chave)
    {
        var notaFiscal = await _repository.GetByChaveAsync(chave);

        if (notaFiscal == null)
            return null;

        return NfeDetailsMapper.ToResponse(notaFiscal);
    }
}