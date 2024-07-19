namespace Brainf1ck_IDE.Common.FileProcessing
{
    public static class Deleter
    {
        public static void TryDeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch
                {
                    // Not critical
                }
            }
        }
    }
}
