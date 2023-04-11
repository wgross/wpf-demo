using System.Runtime.InteropServices;

namespace DemoApp.Hosting
{
    internal static class Win32
    {
        // see PInvoke.Net: https://www.pinvoke.net/default.aspx/kernel32.setstdhandle
        internal const int StdOutputHandle = -11;

        internal static bool AttachToParentConsole() => AttachConsole(uint.MaxValue);

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        internal static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        internal static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        internal static extern bool SetStdHandle(int nStdHandle, IntPtr handle);
    }
}