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
        IndexOutOfMemoryRange,
        // TODO
    }

    public enum BrainfuckOutputTypes : ushort
    {
        Output,
        Warning,
        Error
    }
}
