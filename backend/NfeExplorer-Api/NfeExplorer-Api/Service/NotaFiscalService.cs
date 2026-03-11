using NfeExplorer_Api.Dto;
using NfeExplorer_Api.Models.Enums;
using NfeExplorer_Api.Parser;
using NfeExplorer_Api.Repository;
using NfeExplorer_Api.Validator;

namespace NfeExplorer_Api.Service;

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
        
        var xmlParseado = NfeParser.Parse()
        
        var transportadora = new TransportadoraResponse
        {
            RazaoSocial = ,
            CNPJ = null,
            CPF = null,
            InscricaoEstadual = null,
            Municipio = null,
            UF = null,
            ModalidadeFrete = ModalidadeFrete.PorContaEmitente
        }
    }

    public Task<ImportNfeResponse?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ImportNfeResponse?> GetByChaveAsync(string chave)
    {
        throw new NotImplementedException();
    }
}