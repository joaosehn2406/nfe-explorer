using NfeExplorer_Api.Dto;
using NfeExplorer_Api.Models;

namespace NfeExplorer_Api.Service;

public interface INotaFiscalService
{
    Task<NotaFiscal> Save(ParseNfeRequest request);
}