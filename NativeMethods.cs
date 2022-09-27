using System.Runtime.InteropServices;

namespace Cafenet {
    internal static class NativeMethods {
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;
        public const uint ES_DISPLAY_REQUIRED = 0x00000002;
        public const uint ES_USER_PRESENT = 0x00000004;
        public const uint ES_AWAYMODE_REQUIRED = 0x00000040;
        public const uint ES_CONTINUOUS = 0x80000000;

        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);
    }
}
