using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Cafenet {
    [StructLayout(LayoutKind.Sequential)]
    internal struct LUID {
        public uint LowPart;
        public long HighPart;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct LUID_AND_ATTRIBUTES {
        public LUID Luid;
        public uint Attributes;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct TOKEN_PRIVILEGES {
        public uint PrivilegeCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public LUID_AND_ATTRIBUTES[] Privileges;
    }

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

        public const uint SE_PRIVILEGE_ENABLED_BY_DEFAULT = 0x00000001;
        public const uint SE_PRIVILEGE_ENABLED = 0x00000002;
        public const uint SE_PRIVILEGE_REMOVED = 0x00000004;
        public const uint SE_PRIVILEGE_USED_FOR_ACCESS = 0x80000000;

        public const uint TOKEN_QUERY = 0x0008;
        public const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;

        public static readonly string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";

        public const uint WM_ACTIVATE = 0x0006;
        public const int SW_HIDE = 0;

        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetCurrentPackageFullName(ref uint packageFullNameLength, StringBuilder packageFullName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint RegisterApplicationRestart(string pwzCommandline, uint dwFlags);

        [DllImport("powrprof.dll", SetLastError = true)]
        public static extern bool SetSuspendState(bool bHibernate, bool bForce, bool bWakeupEventsDisabled);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        public static extern bool LookupPrivilegeValue(string lpSystemName, string lpName, out LUID lpLUID);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool AdjustTokenPrivileges(IntPtr TokenHandle, bool DisableAllPrivileges, ref TOKEN_PRIVILEGES NewState, uint BufferLength, IntPtr PreviousState, IntPtr ReturnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("comctl32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowSubclass(IntPtr hWnd, IntPtr pfnSubclass, UIntPtr uIdSubclass, UIntPtr dwRefData);
        [DllImport("comctl32.dll")]
        public static extern IntPtr DefSubclassProc(IntPtr hWnd, uint uMsg, UIntPtr wParam, IntPtr lParam);
        [DllImport("comctl32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RemoveWindowSubclass(IntPtr hWnd, IntPtr pfnSubclass, UIntPtr uIdSubclass);
    }

    internal static class NativeUtilities {
        public static void EnableShutdownPrivilege(IntPtr handle, bool enable) {
            var tp = new TOKEN_PRIVILEGES {
                PrivilegeCount = 1,
                Privileges = new LUID_AND_ATTRIBUTES[1],
            };
            tp.Privileges[0].Attributes = enable ? NativeMethods.SE_PRIVILEGE_ENABLED : NativeMethods.SE_PRIVILEGE_REMOVED;
            if (!NativeMethods.LookupPrivilegeValue(null, NativeMethods.SE_SHUTDOWN_NAME, out tp.Privileges[0].Luid)) {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "LookupPrivilegeValue(SE_SHUTDOWN_NAME)");
            }
            if (!NativeMethods.AdjustTokenPrivileges(handle, false, ref tp, (uint)Marshal.SizeOf(tp), IntPtr.Zero, IntPtr.Zero)) {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "AdjustTokenPrivileges");
            }
        }

        public static void SuspendSystem() {
            if (!NativeMethods.OpenProcessToken(Process.GetCurrentProcess().Handle, NativeMethods.TOKEN_QUERY | NativeMethods.TOKEN_ADJUST_PRIVILEGES, out var token)) {
                throw new Win32Exception(Marshal.GetLastWin32Error(), "OpenProcessToken");
            }
            try {
                EnableShutdownPrivilege(token, true);
                try {
                    NativeMethods.SetSuspendState(false, false, false);
                } finally {
                    EnableShutdownPrivilege(token, false);
                }
            } finally {
                NativeMethods.CloseHandle(token);
            }
        }
    }
}
