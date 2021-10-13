using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace HookTest
{
    public enum DllMainFlags : uint
    {
        DLL_PROCESS_DETACH = 0,
        DLL_PROCESS_ATTACH = 1,
        DLL_THREAD_ATTACH = 2,
        DLL_THREAD_DETACH = 3,
    }

    public enum BOOL : int
    {
        FALSE = 0,
        TRUE = 1
    }

    public class Win32
    {
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int MessageBoxA(IntPtr hWnd, string text, string caption, uint type);

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern void OutputDebugStringA(string lpOutputString);

        [DllImport("kernel32.dll")]
        public static extern BOOL AllocConsole();

        [DllImport("kernel32.dll")]
        public static extern BOOL FreeConsole();


        public enum CtrlType : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6,
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate BOOL HandlerRoutine(CtrlType dwCtrlType);

        [DllImport("kernel32.dll")]
        public static extern BOOL SetConsoleCtrlHandler([MarshalAs(UnmanagedType.FunctionPtr)] HandlerRoutine HandlerRoutine, BOOL Add);
    }

    public static class HookTest
    {
        static HookTest() {
            Win32.AllocConsole();
            Console.WriteLine("I'm in (⌐■_■)");

            Win32.SetConsoleCtrlHandler((type) => {
                switch (type)
                {
                    case Win32.CtrlType.CTRL_C_EVENT:
                        Console.WriteLine("NO CTRLC!");
                        return BOOL.TRUE;
                }

                return BOOL.FALSE;
            }, BOOL.TRUE);

            new Thread(() => {
                
            }).Start();
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate int DirectInput8CreateDelegate(IntPtr hInst, uint dwVersion, IntPtr riidltf, IntPtr ppvOut, IntPtr punkOuter);

        [DllExport]
        public static int DirectInput8Create(IntPtr hInst, uint dwVersion, IntPtr riidltf, IntPtr ppvOut, IntPtr punkOuter)
        {
            var realDllPath = Environment.SystemDirectory + "\\dinput8.dll";
            var realDll = Win32.LoadLibrary(realDllPath);

            var realAddress = Win32.GetProcAddress(realDll, "DirectInput8Create");
            var original = Marshal.GetDelegateForFunctionPointer<DirectInput8CreateDelegate>(realAddress);

            return original(hInst, dwVersion, riidltf, ppvOut, punkOuter);
        }


        //[DllExport(CallingConvention = CallingConvention.StdCall)]
        //public static int DllMain(IntPtr hModule, uint fdwReason, IntPtr lpReserved)
        //{
        //    Loaded = true;
        //
        //    switch (fdwReason)
        //    {
        //        case 1:
        //            break;
        //    }
        //
        //    return 1;
        //}
    }
}
