using System.Runtime.InteropServices;

namespace Console_MultipleMonitors_ExtendDisplays;

public static class MonitorNativeMethods
{
    private const int CCHDEVICENAME = 32;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumProc lpfnEnum, IntPtr dwData);
    
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);
    
    public delegate bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);
}

public class MonitorInfo
{
    public IntPtr Handle { get; set; }
    public RECT MonitorArea { get; set; }
    public RECT WorkArea { get; set; }
    public bool IsPrimary { get; set; }
}

[StructLayout(LayoutKind.Sequential)]
public struct RECT
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public struct MONITORINFO
{
    public int Size;
    public RECT Monitor;
    public RECT Work;
    public uint Flags;
}