namespace NfeExplorer_Api.Dto;

public class ParseNfeRequest
{
    public IFormFile? File { get; set; }
    public string XmlText { get; set; }

    public bool IsValid => File != null || !string.IsNullOrWhiteSpace(XmlText);
}