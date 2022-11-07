using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

using consoleGraphicsEngine;
using static consoleGraphicsEngine.Structs;
using static consoleGraphicsEngine.Drawing;

namespace consoleGraphicsEngine
{
    public static class Globals
    {
        //sets the default window size
        public static short DISP_X = 80;
        public static short DISP_Y = 20;

        public static CharInfo[] buf = new CharInfo[Globals.DISP_X * Globals.DISP_Y];
        public static SmallRect rect = new SmallRect() { Left = 0, Top = 0, Right = Globals.DISP_X, Bottom = Globals.DISP_Y };

        public static int lastSceneChangeFrame = 0;

        public static SafeFileHandle? hndl;
    }

    public static class Structs
    {
        //CPP: COORD struct
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }

            public Coord(int X, int Y)
            {
                this.X = (short)X;
                this.Y = (short)Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public byte AsciiChar;
            [FieldOffset(0)] public short UnicodeChar;
            [FieldOffset(2)] public short Attributes;
        }

        //CPP: RECT struct (windef.h)
        //changed to short from long
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }

        //struct for string alignment settings
        public enum stAlignment
        {
            LEFT,
            CENTER,
            RIGHT
        }
    }

    public static class Colours
    {
        //basic windows colours
        public static byte BLACK = 0x00;
        public static byte BLUE = 0x10;
        public static byte GREEN = 0x20;
        public static byte AQUA = 0x30;
        public static byte RED = 0x40;
        public static byte PURPLE = 0x50;
        public static byte YELLOW = 0x60;
        public static byte WHITE = 0x70;
        public static byte GRAY = 0x80;
        public static byte LBLUE = 0x90;
        public static byte LGREEN = 0xA0;
        public static byte LAQUA = 0xB0;
        public static byte LRED = 0xC0;
        public static byte LPURPLE = 0xD0;
        public static byte LYELLOW = 0xE0;
        public static byte LWHITE = 0xF0;
    }

    public static class Keys
    {

        //keycodes
        public static byte VK_LEFT = 0x25;
        public static byte VK_RIGHT = 0x27;
        public static byte VK_UP = 0x26;
        public static byte VK_SPACE = 0x20;
        public static byte VK_ESCAPE = 0x1B;
    }
}