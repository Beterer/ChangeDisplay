using System;
using System.Runtime.InteropServices;
using static ChangeDisplay.PrintersettingsTest.PrinterSettings;

namespace DisplayDevices
{
    public class DisplayDevices
    {
        [DllImport("user32.dll")]
        static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll")]
        static extern DISP_CHANGE ChangeDisplaySettingsEx(string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd, ChangeDisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool EnumDisplaySettingsEx(string lpszDeviceName, uint iModeNum, out DEVMODE lpDevMode, uint dwFlags);


        public void Display()
        {
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE dm = new DEVMODE();

            d.cb = Marshal.SizeOf(d);
            try
            {
                for (uint id = 0; EnumDisplayDevices(null, id, ref d, 0); id++)
                {
                    if (d.StateFlags.HasFlag(DisplayDeviceStateFlags.AttachedToDesktop))
                    {
                        Console.WriteLine(
                            String.Format("{0}, {1}, {2}, {3}, {4}, {5}",
                                     id,
                                     d.DeviceName,
                                     d.DeviceString,
                                     d.StateFlags,
                                     d.DeviceID,
                                     d.DeviceKey
                                     )
                                     );
                        d.cb = Marshal.SizeOf(d);
                        EnumDisplayDevices(d.DeviceName, 0, ref d, 0);
                        EnumDisplaySettingsEx(d.DeviceName, 0, out dm, 0);

                        Console.WriteLine(
                            String.Format("{0}, {1}",
                                     d.DeviceName,
                                     d.DeviceString
                                     )
                                     );

                        ChangeDisplaySettingsEx(d.DeviceName, ref dm, IntPtr.Zero, ChangeDisplaySettingsFlags.CDS_GLOBAL, IntPtr.Zero);
                    }
                    d.cb = Marshal.SizeOf(d);
                }
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Format("{0}", ex.ToString()));
            }
        }
    }
}
