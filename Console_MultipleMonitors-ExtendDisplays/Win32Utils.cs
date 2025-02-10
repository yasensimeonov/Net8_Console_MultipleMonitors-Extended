using System.Management;
using System.Runtime.InteropServices;

namespace Console_MultipleMonitors_ExtendDisplays;

public static class Win32Utils
{
    public static void QueryWmiForMonitors()
    {
        var monitorQuery = new ManagementObjectSearcher("SELECT * FROM Win32_DesktopMonitor");
        foreach (ManagementObject monitor in monitorQuery.Get())
        {
            Console.WriteLine($"Monitor: {monitor["Name"]}");
            Console.WriteLine($"Status: {monitor["Status"]}");
            Console.WriteLine($"Availability: {monitor["Availability"]}");
        }

        var videoControllerQuery = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
        foreach (ManagementObject videoController in videoControllerQuery.Get())
        {
            Console.WriteLine($"Video Controller: {videoController["Name"]}");
            Console.WriteLine($"Status: {videoController["Status"]}");
        }
    }

    public static List<MonitorInfo> GetMonitors()
    {
        var monitors = new List<MonitorInfo>();

        MonitorNativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData) =>
        {
            var mi = new MONITORINFO();
            mi.Size = Marshal.SizeOf(mi);
            if (MonitorNativeMethods.GetMonitorInfo(hMonitor, ref mi))
            {
                monitors.Add(new MonitorInfo
                {
                    Handle = hMonitor,
                    MonitorArea = mi.Monitor,
                    WorkArea = mi.Work,
                    IsPrimary = (mi.Flags & 1) == 1 // MONITORINFOF_PRIMARY
                });
            }
            return true;
        }, IntPtr.Zero);

        return monitors;
    }

    public static void CheckExtendedMode()
    {
        var monitors = GetMonitors();
        if (monitors.Count > 1)
        {
            Console.WriteLine("Multiple monitors detected.");
            bool isExtended = false;

            for (int i = 0; i < monitors.Count; i++)
            {
                for (int j = i + 1; j < monitors.Count; j++)
                {
                    if (monitors[i].MonitorArea.Right <= monitors[j].MonitorArea.Left ||
                        monitors[i].MonitorArea.Left >= monitors[j].MonitorArea.Right)
                    {
                        isExtended = true;
                        break;
                    }
                }
                if (isExtended) break;
            }

            if (isExtended)
            {
                Console.WriteLine("Monitors are in 'Extend these displays' mode.");
            }
            else
            {
                Console.WriteLine("Monitors are not in 'Extend these displays' mode.");
            }
        }
        else
        {
            Console.WriteLine("Only one monitor detected.");
        }
    }

}
