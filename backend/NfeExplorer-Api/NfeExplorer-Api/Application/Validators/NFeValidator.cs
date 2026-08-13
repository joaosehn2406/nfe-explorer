using System.Xml.Linq;
using NfeExplorer_Api.Application.DTOs.Requests;

namespace NfeExplorer_Api.Application.Validators;

public static class NFeValidator
{
    private const long MaxFileSize = 5 * 1024 * 1024;

    public static void ValidateRequest(ParseNfeRequest request)
    {
        if (request == null)
        {
            throw new ArgumentException("Invalid request.");
        }

        ValidateXmlSource(request);

        if (request.File != null)
        {
            if (request.File.Length == 0)
            {
                throw new ArgumentException("Empty file.");
            }

            if (request.File.Length > MaxFileSize)
            {
                throw new ArgumentException("File exceeds the maximum allowed size.");
            }

            ValidateFileExtension(request);
        }
    }

    public static void ValidateXml(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
        {
            throw new ArgumentException("Empty XML.");
        }

        XDocument document;

        try
        {
            document = XDocument.Parse(xml);
        }
        catch
        {
            throw new ArgumentException("Invalid XML.");
        }

        ValidateNFeStructure(document);
    }

    private static void ValidateXmlSource(ParseNfeRequest request)
    {
        if (request.File == null && string.IsNullOrWhiteSpace(request.XmlText))
        {
            throw new ArgumentException("Provide an XML file or paste XML content to continue.");
        }
    }

    private static void ValidateFileExtension(ParseNfeRequest request)
    {
        if (request.File == null)
        {
            return;
        }

        var extension = Path.GetExtension(request.File.FileName);

        if (!extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Only XML files can be imported.");
        }
    }

    private static void ValidateNFeStructure(XDocument document)
    {
        var infNfe = document.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "infNFe");

        if (infNfe == null)
        {
            throw new ArgumentException("XML does not contain a valid NF-e.");
        }

        var ide = infNfe.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "ide");

        var emit = infNfe.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "emit");

        var dest = infNfe.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "dest");

        var total = infNfe.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "total");

        if (ide == null)
        {
            throw new ArgumentException("Invalid NF-e structure: ide was not found.");
        }

        if (emit == null)
        {
            throw new ArgumentException("Invalid NF-e structure: emit was not found.");
        }

        if (dest == null)
        {
            throw new ArgumentException("Invalid NF-e structure: dest was not found.");
        }

        if (total == null)
        {
            throw new ArgumentException("Invalid NF-e structure: total was not found.");
        }
    }
}
