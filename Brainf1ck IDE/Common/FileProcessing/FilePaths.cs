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

        public static async Task<FileResult?> PickFilePath()
        {
            var chosenFile = await FilePicker.PickAsync(
                new PickOptions
                {
                    PickerTitle = $"Please select file with " +
                    $"\"{ideRelatedFileExtension}\" extension",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        {DevicePlatform.Android, new[] { FilePaths.ideRelatedFileExtension } },
                        {DevicePlatform.WinUI, new[] { FilePaths.ideRelatedFileExtension } }
                    })
                });
            return chosenFile;
        }
    }
}
