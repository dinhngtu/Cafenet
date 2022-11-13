using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Cafenet {
    internal static class NativeMethods {
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;
        public const uint ES_DISPLAY_REQUIRED = 0x00000002;
        public const uint ES_USER_PRESENT = 0x00000004;
        public const uint ES_AWAYMODE_REQUIRED = 0x00000040;
        public const uint ES_CONTINUOUS = 0x80000000;

        public const int APPMODEL_ERROR_NO_PACKAGE = 0x3d54;
        public const int ERROR_SUCCESS = 0;
        public const int ERROR_INSUFFICIENT_BUFFER = 0x7a;

        public const uint RESTART_NO_CRASH = 1;
        public const uint RESTART_NO_HANG = 2;
        public const uint RESTART_NO_PATCH = 4;
        public const uint RESTART_NO_REBOOT = 8;

        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetCurrentPackageFullName(ref uint packageFullNameLength, StringBuilder packageFullName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint RegisterApplicationRestart(string pwzCommandline, uint dwFlags);
    }
}
