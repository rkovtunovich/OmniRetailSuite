namespace Helpers.IOHelper;

public static class FileVersionHelper
{
    public static string GetFileVersion(string filePath)
    {
        if (File.Exists(filePath))
        {
            var lastWriteTime = File.GetLastWriteTimeUtc(filePath);
            return lastWriteTime.ToString("yyyyMMddHHmmss");
        }
        return "00000000000000"; // Default version if file not found
    }
}
