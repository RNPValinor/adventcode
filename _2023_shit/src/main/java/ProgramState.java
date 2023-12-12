public class ProgramState {
    private static boolean IsDebugMode;

    public static boolean IsDebugMode()
    {
        return IsDebugMode;
    }

    public static void EnableDebug()
    {
        IsDebugMode = true;
    }
}
