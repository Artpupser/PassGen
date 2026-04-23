using System.Runtime.InteropServices;

namespace PassGen.Static;

internal static partial class Kernel {
   [LibraryImport("kernel32.dll", SetLastError = true)]
   [return: MarshalAs(UnmanagedType.Bool)]
   private static partial bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

   [LibraryImport("kernel32.dll", SetLastError = true)]
   [return: MarshalAs(UnmanagedType.Bool)]
   private static partial bool GetConsoleMode(IntPtr handle, out int mode);

   [LibraryImport("kernel32.dll", SetLastError = true)]
   private static partial IntPtr GetStdHandle(int handle);

   internal static void ConsoleInit() {
      var handle = GetStdHandle(-11);
      GetConsoleMode(handle, out var mode);
      SetConsoleMode(handle, mode | 0x4);
   }
}