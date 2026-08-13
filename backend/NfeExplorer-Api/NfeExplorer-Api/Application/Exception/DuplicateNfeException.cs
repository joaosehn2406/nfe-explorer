namespace NfeExplorer_Api.Application.Exception;

/// <summary>
/// Thrown when an NF-e with the same access key is imported again.
/// </summary>
public class DuplicateNfeException : System.Exception
{
    public DuplicateNfeException(string message) : base(message) { }
}
