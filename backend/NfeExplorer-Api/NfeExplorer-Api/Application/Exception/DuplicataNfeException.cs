namespace NfeExplorer_Api.Application.Exception;

/// <summary>
/// Lançada quando uma NF-e com chave de acesso já existente é importada novamente.
/// Mapeada para HTTP 409 (Conflict) pelo GlobalExceptionHandler.
/// </summary>
public class DuplicataNfeException : System.Exception
{
    public DuplicataNfeException(string message) : base(message) { }
}
