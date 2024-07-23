namespace Brainf1ck_IDE.Common
{
    public enum ProjectInvalidityTypes : ushort
    {
        InputLack,
        WrongInput,
        InvalidName,
        InvalidProperties
    }

    public enum BrainfuckErrorTypes : ushort
    {
        NoError,
        IndexOutOfMemoryRange,
        NotBalancedSquareBrackets,
        NoParsableSymbolsFound,
        NoInput
    }

    public enum BrainfuckOutputTypes : ushort
    {
        Output,
        Warning,
        Error
    }

    public enum CodeSnippets : ushort
    {
        HelloWorld,
        // TODO add more snippets
        Cancel
    }
}
