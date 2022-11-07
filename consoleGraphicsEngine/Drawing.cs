using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using consoleGraphicsEngine;
using static consoleGraphicsEngine.Structs;
using static consoleGraphicsEngine.Drawing;


namespace consoleGraphicsEngine
{
    public static class Drawing
    {
        //draws a single pixel in a defined point
        public static void drawPixel(byte colour, Coord position, char fillChar)
        {
            int pxPositionInBuffer = (position.Y * Globals.DISP_X) + position.X;
            if (pxPositionInBuffer >= Globals.buf.Length) { return; }
            Globals.buf[pxPositionInBuffer].Attributes = colour;
            Globals.buf[pxPositionInBuffer].AsciiChar = (byte)fillChar;
        }

        //merges the colours above into a single byte
        //console colours use the first 4 bytes to define the background colour and the last 4 bytes to define the text colour
        static byte mergeColours(byte background, byte text)
        {
            //merges two colours into one windows-recognised code by shifting the text byte 4 to the right and then ORing it with the background byte
            return (byte)((text >> 4) ^ background);
        }

        //fills the display with one colour
        public static void clearDisplay(byte colourSet)
        {
            for (int i = 0; i < Globals.buf.Length; ++i)
            {
                Globals.buf[i].Attributes = colourSet;
                Globals.buf[i].AsciiChar = 0x00;
            }
        }

        //draws lines using bresenhams algorithm
        //https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        public static void drawLine(byte color, Coord p1, Coord p2, char fillChar)
        {
            short w = (short)(p2.X - p1.X);
            short h = (short)(p2.Y - p1.Y);
            short dx1 = 0;
            short dy1 = 0;
            short dx2 = 0;
            short dy2 = 0;
            if (w < 0) { dx1 = -1; } else if (w > 0) { dx1 = 1; };
            if (h < 0) { dy1 = -1; } else if (h > 0) { dy1 = 1; };
            if (w < 0) { dx2 = -1; } else if (w > 0) { dx2 = 1; };
            short longest = Math.Abs(w);
            short shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) { dy2 = -1; } else if (h > 0) { dy2 = 1; };
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                drawPixel(color, new Coord(p1.X, p1.Y), fillChar);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    p1.X += dx1;
                    p1.Y += dy1;
                }
                else
                {
                    p1.X += dx2;
                    p1.Y += dy2;
                }
            }
        }

        //draws strings, can align to center, left or right
        public static void drawString(string text, Coord pos, byte textColour, stAlignment alignment = stAlignment.LEFT, bool wrap = true)
        {
            int x = 0;
            int y = 0;

            if (alignment == stAlignment.LEFT)
            {
                x = pos.X;
                y = pos.Y;
            }
            if (alignment == stAlignment.CENTER)
            {
                x = (short)(pos.X - (short)(text.Length / 2));
                y = pos.Y;
            }
            if (alignment == stAlignment.RIGHT)
            {
                x = (short)(pos.X - (short)text.Length);
                y = pos.Y;
            }

            int startPositionInBuffer = (y * Globals.DISP_X) + x;
            for (int i = 0; i < text.Length; ++i)
            {
                if (startPositionInBuffer + i > Globals.buf.Length || startPositionInBuffer + i < 0) { return; }
                if (!wrap && (startPositionInBuffer + i) % Globals.DISP_X == 0) { return; } //imperfect when string is centered
                Globals.buf[startPositionInBuffer + i].Attributes = mergeColours((byte)Globals.buf[startPositionInBuffer + i].Attributes, textColour); //keeps the previous background colour
                Globals.buf[startPositionInBuffer + i].AsciiChar = (byte)text[i];
            }
        }

        //draws a rectangle
        public static void drawRect(Coord pos1, Coord pos2, byte colour, char fillChar)
        {
            drawLine(colour, new Coord(pos1.X, pos1.Y), new Coord(pos2.X, pos1.Y), (char)0x00);

            //double left lines
            drawLine(colour, new Coord(pos1.X, pos1.Y), new Coord(pos1.X, pos2.Y), (char)0x00);
            drawLine(colour, new Coord(pos1.X + 1, pos1.Y), new Coord(pos1.X + 1, pos2.Y), (char)0x00);

            //double right lines
            drawLine(colour, new Coord(pos2.X, pos2.Y), new Coord(pos2.X, pos1.Y), (char)0x00);
            drawLine(colour, new Coord(pos2.X - 1, pos2.Y), new Coord(pos2.X - 1, pos1.Y), (char)0x00);

            //bottom line
            drawLine(colour, new Coord(pos1.X, pos2.Y), new Coord(pos2.X, pos2.Y), (char)0x00);
        }

    }
}
