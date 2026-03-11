using NfeExplorer_Api.Dto;
using NfeExplorer_Api.Repository;

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
        string xml;

        if (request.IsValid)
        {
            if (request.File != null)
            {
                using var stream = request.File.OpenReadStream();
                using var reader = new StreamReader(stream);
                xml = await reader.ReadToEndAsync();   
            }
            else
            {
                xml = request.XmlText;
            }
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