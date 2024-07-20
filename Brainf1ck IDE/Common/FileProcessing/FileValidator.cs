using System.Text.RegularExpressions;

namespace Brainf1ck_IDE.Common.FileProcessing
{
    public static class FileValidator
    {
        public static bool IsStringValidForFilesystem(string fileName)
        {
            Regex regex = new("^[a-zA-Z0-9_]+$");
            return !string.IsNullOrWhiteSpace(fileName)
                && regex.IsMatch(fileName);
        }

        public static bool IsRootFolderValid(string? path)
        {
            return path is not null && Directory.Exists(path);
        }
    }
}
