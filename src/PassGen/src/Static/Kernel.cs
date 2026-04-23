using System.Runtime.InteropServices;

namespace PassGen.Static;

internal static partial class Kernel {
   [DllImport( "kernel32.dll", SetLastError = true )]
   private static extern bool SetConsoleMode( IntPtr hConsoleHandle, int mode );
   [DllImport( "kernel32.dll", SetLastError = true )]
   private static extern bool GetConsoleMode( IntPtr handle, out int mode );

   [DllImport( "kernel32.dll", SetLastError = true )]
   private static extern IntPtr GetStdHandle( int handle );

   internal static void ConsoleInit() {
      var handle = GetStdHandle(-11);
      GetConsoleMode(handle, out var mode);
      SetConsoleMode(handle, mode | 0x4);
   }
}