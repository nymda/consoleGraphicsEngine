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

            public SmallRect(short Left, short Top, short Right, short Bottom)
            {
                this.Left = Left;
                this.Top = Top;
                this.Right = Right;
                this.Bottom = Bottom;
            }

        }

        //struct for string alignment settings
        public enum stAlignment
        {
            LEFT,
            CENTER,
            RIGHT
        }
    }

    public struct winColour
    {
        public int R;
        public int G;
        public int B;
        public byte Code;

        public winColour(int R, int G, int B, byte Code)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.Code = Code;
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

    public static class winColours
    {
        //basic windows colours
        public static winColour BLACK = new winColour( 0x0c, 0x0c, 0x0c, Colours.BLACK );
        public static winColour BLUE = new winColour( 0x00, 0x37, 0xda, Colours.BLUE);
        public static winColour GREEN = new winColour( 0x13, 0xa1, 0x0e, Colours.GREEN);
        public static winColour AQUA = new winColour( 0x3a, 0x96, 0xdd, Colours.AQUA);
        public static winColour RED = new winColour( 0xc5, 0x0f, 0x1f, Colours.RED);
        public static winColour PURPLE = new winColour( 0x88, 0x17, 0x98, Colours.PURPLE);
        public static winColour YELLOW = new winColour( 0xc1, 0x9c, 0x00, Colours.YELLOW);
        public static winColour WHITE = new winColour( 0xcc, 0xcc, 0xcc, Colours.WHITE);
        public static winColour GRAY = new winColour( 0x76, 0x76, 0x76, Colours.GRAY);
        public static winColour LBLUE = new winColour( 0x3b, 0x78, 0xff, Colours.LBLUE);
        public static winColour LGREEN = new winColour( 0x16, 0xc6, 0x0c, Colours.LGREEN);
        public static winColour LAQUA = new winColour( 0x61, 0xd6, 0xd6, Colours.LAQUA);
        public static winColour LRED = new winColour( 0xe7, 0x48, 0x56, Colours.LRED);
        public static winColour LPURPLE = new winColour( 0xb4, 0x00, 0x9e, Colours.LPURPLE);
        public static winColour LYELLOW = new winColour( 0xf9, 0xf1, 0xa5, Colours.LYELLOW);
        public static winColour LWHITE = new winColour( 0xf2, 0xf2, 0xf2, Colours.LWHITE);

        public static readonly winColour[] LIST = { BLACK, BLUE, GREEN, AQUA, RED, PURPLE, YELLOW, WHITE, GRAY, LBLUE, LGREEN, LAQUA, LRED, LPURPLE, LYELLOW, LWHITE };
        public static readonly winColour[] LIST_NOPRIM = { BLUE, GREEN, AQUA, RED, PURPLE, YELLOW, LBLUE, LGREEN, LAQUA, LRED, LPURPLE, LYELLOW };
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