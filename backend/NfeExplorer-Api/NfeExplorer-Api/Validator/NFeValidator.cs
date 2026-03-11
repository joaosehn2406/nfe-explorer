using System.Xml.Linq;
using NfeExplorer_Api.Dto;

namespace NfeExplorer_Api.Validator;

public static class NFeValidator
{
    private const long MaxFileSize = 5 * 1024 * 1024;

    public static void ValidarRequest(ParseNfeRequest request)
    {
        if (request == null)
            throw new ArgumentException("Requisição inválida.");

        ValidarPresencaDeXmlOuArquivo(request);

        if (request.File != null)
        {
            if (request.File.Length == 0)
                throw new ArgumentException("Arquivo vazio.");

            if (request.File.Length > MaxFileSize)
                throw new ArgumentException("Arquivo excede o tamanho máximo permitido.");

            ValidarExtensaoArquivo(request);
        }
    }

    public static void ValidarXml(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            throw new ArgumentException("XML vazio.");

        XDocument document;

        try
        {
            document = XDocument.Parse(xml);
        }
        catch
        {
            throw new ArgumentException("XML inválido.");
        }

        ValidarEstruturaNFe(document);
    }

    private static void ValidarPresencaDeXmlOuArquivo(ParseNfeRequest request)
    {
        if (request.File == null && string.IsNullOrWhiteSpace(request.XmlText))
            throw new ArgumentException("Para prosseguir, insira um XML ou arquivo.");
    }

    private static void ValidarExtensaoArquivo(ParseNfeRequest request)
    {
        if (request.File == null)
            return;

        var extension = Path.GetExtension(request.File.FileName);

        if (!extension.Equals(".xml", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Importação permitida apenas para arquivos XML.");
    }

    private static void ValidarEstruturaNFe(XDocument document)
    {
        var infNfe = document.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "infNFe");

        if (infNfe == null)
            throw new ArgumentException("XML não contém uma NF-e válida.");

        var ide = infNfe.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "ide");

        var emit = infNfe.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "emit");

        var dest = infNfe.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "dest");

        var total = infNfe.Descendants()
            .FirstOrDefault(e => e.Name.LocalName == "total");

        if (ide == null)
            throw new ArgumentException("Estrutura da NF-e inválida: ide não encontrado.");

        if (emit == null)
            throw new ArgumentException("Estrutura da NF-e inválida: emit não encontrado.");

        if (dest == null)
            throw new ArgumentException("Estrutura da NF-e inválida: dest não encontrado.");

        if (total == null)
            throw new ArgumentException("Estrutura da NF-e inválida: total não encontrado.");
    }
}