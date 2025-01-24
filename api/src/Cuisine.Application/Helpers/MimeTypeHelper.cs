namespace Cuisine.Application.Helpers;

public class MimeTypeHelper
{
    private static readonly HashSet<string> AllowedMimeTypes = new HashSet<string>
    {
        "image/jpeg",
        "image/png",
        "image/gif",
        "application/pdf",
        "text/plain"
    };

    private static readonly Dictionary<string, string> MimeMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { ".jpg", "image/jpeg" },
        { ".jpeg", "image/jpeg" },
        { ".png", "image/png" },
        { ".gif", "image/gif" },
        { ".pdf", "application/pdf" },
        { ".txt", "text/plain" }
    };

    public static string GetMimeType(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("Le fichier spécifié est introuvable.", filePath);

        string extension = Path.GetExtension(filePath);

        if (!MimeMapping.TryGetValue(extension, out var mimeType))
            throw new NotSupportedException($"L'extension {extension} n'est pas prise en charge.");

        if (!AllowedMimeTypes.Contains(mimeType))
            throw new NotSupportedException($"Le type MIME {mimeType} est interdit.");

        return mimeType;
    }
}
