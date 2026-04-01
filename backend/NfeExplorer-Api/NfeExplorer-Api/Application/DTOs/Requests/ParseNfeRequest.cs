namespace NfeExplorer_Api.Application.DTOs.Requests;

public class ParseNfeRequest
{
    public IFormFile? File { get; set; }
    public string? XmlText { get; set; }
}