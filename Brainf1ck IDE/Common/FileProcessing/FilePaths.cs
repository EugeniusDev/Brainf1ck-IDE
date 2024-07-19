namespace Brainf1ck_IDE.Common.FileProcessing
{
    public static class FilePaths
    {
        public const string ideRelatedFileExtension = ".bfide";
        public const string welcomeScriptFileName = "main.bf";

        public static readonly string projectsDataList = Path.Combine(
            FileSystem.AppDataDirectory, $"projectsList{ideRelatedFileExtension}");
        public static readonly string globalSettingsPath = Path.Combine(
            FileSystem.AppDataDirectory, $"ideSettings{ideRelatedFileExtension}");

        public static string GetProjectSettingsFilename(string projectName)
        {
            return $"{projectName}{ideRelatedFileExtension}";
        }
    }
}
