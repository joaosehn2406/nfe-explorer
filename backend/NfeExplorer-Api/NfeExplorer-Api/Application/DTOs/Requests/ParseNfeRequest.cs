namespace NfeExplorer_Api.Application.DTOs.Requests;

public class ParseNfeRequest
{
    public IFormFile? File { get; }
    public string? XmlText { get; }
}