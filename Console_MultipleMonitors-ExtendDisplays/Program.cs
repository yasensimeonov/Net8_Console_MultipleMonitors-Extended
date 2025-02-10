namespace Console_MultipleMonitors_ExtendDisplays;

internal class Program
{
    static void Main(string[] args)
    {
        Win32Utils.QueryWmiForMonitors();
        Win32Utils.CheckExtendedMode();
    }
}
