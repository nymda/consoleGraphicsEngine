using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using consoleGraphicsEngine;
using static consoleGraphicsEngine.Structs;
using static consoleGraphicsEngine.Drawing;

namespace consoleGameEngine
{
    class Program
    {
        #region dllimports
        //imports and structs, AKA stuff that C# should have already

        //import createFile from kernel32.dll
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        //import writeConsoleOutputW from kernel32.dll
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutputW(
          SafeFileHandle hConsoleOutput,
          CharInfo[] lpBuffer,
          Coord dwBufferSize,
          Coord dwBufferCoord,
          ref SmallRect lpWriteRegion);

        //import GetKeyState from User32.dll
        [DllImport("User32.dll")]
        public static extern short GetKeyState(byte keycode);

        #endregion

        //draws whatevers in the display buffer to the display
        public static bool update()
        {
            return WriteConsoleOutputW(Globals.hndl, Globals.buf, new Coord() { X = Globals.DISP_X, Y = Globals.DISP_Y }, new Coord() { X = 0, Y = 0 }, ref Globals.rect);
        }

        static void logSceneChange(int frame)
        {
            Globals.lastSceneChangeFrame = frame;
        }

        static void Main(string[] args)
        {
            //get handle to the consoles output directly
            //this is used with WriteConsoleOutputW to avoid having to use high level functions like Console.writeLine
            Globals.hndl = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            if (Globals.hndl == null) { return; }

            //set up the consoles size
            Console.SetWindowSize(Globals.DISP_X, Globals.DISP_Y);
            Console.CursorVisible = false;
            Console.Title = "UI tests";

            int frameNum = 0;
            byte borderColour = Colours.WHITE;

            while (true)
            {
                if (!Globals.hndl.IsInvalid)
                {
                    //handles if the user resizes the window
                    if (Console.WindowWidth != Globals.DISP_X || Console.WindowHeight != Globals.DISP_Y)
                    {
                        Globals.DISP_X = (short)Console.WindowWidth;
                        Globals.DISP_Y = (short)Console.WindowHeight;
                        Globals.buf = new CharInfo[Globals.DISP_X * Globals.DISP_Y];
                        Globals.rect = new SmallRect() { Left = 0, Top = 0, Right = Globals.DISP_X, Bottom = Globals.DISP_Y };
                        Console.CursorVisible = false;
                        Console.Clear();
                    }

                    clearDisplay(Colours.BLACK);
                    drawRect(new Coord(0, 0), new Coord(Globals.DISP_X - 1, Globals.DISP_Y - 1), borderColour, (char)0x00);
                    drawString("Template", new Coord(2, 0), Colours.BLACK);

                    //update the display
                    update();

                    //run at a target of 20fps
                    Thread.Sleep(50);
                }
                else
                {
                    return;
                }
            }
        }
    }
}