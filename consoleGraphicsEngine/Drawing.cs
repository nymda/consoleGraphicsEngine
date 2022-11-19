using static consoleGraphicsEngine.Structs;
using System.Drawing;

namespace consoleGraphicsEngine
{
    public static class Drawing
    {
        public static void rasterize()
        {
            //prepares the primary buffer for recieving the two-line-per-char graphics data
            for (int i = 0; i < Globals.primaryBuffer.Length; ++i)
            {
                Globals.primaryBuffer[i].Attributes = mergeColours(Colours.BLACK, Colours.BLACK);
                Globals.primaryBuffer[i].UnicodeChar = (short)'▄';
            }

            //write data from graphics buffer into primary buffer
            int cY = 0;
            for (int gpY = 0; gpY < Globals.DISP_Y * 2; ++gpY)
            {
                for (int gpX = 0; gpX < Globals.DISP_X; ++gpX)
                {
                    int graphicalPX = (gpY * Globals.DISP_X) + gpX;
                    int primaryPX = (cY * Globals.DISP_X) + gpX;
                    byte currentCharAttri = (byte)Globals.primaryBuffer[primaryPX].Attributes;
                    byte graphicsBufferPx = Globals.graphicsBuffer[graphicalPX];

                    if (gpY % 2 != 0) //char row
                    {
                        Globals.primaryBuffer[primaryPX].Attributes = mergeColours(extractBackground(currentCharAttri), graphicsBufferPx);
                    }
                    else //background row
                    {
                        Globals.primaryBuffer[primaryPX].Attributes = mergeColours(graphicsBufferPx, extractText(currentCharAttri));
                    }

                }
                if(gpY % 2 != 0)
                {
                    cY++;
                }
            }

            //write data from text buffer into the primary buffer
            for(int tptr = 0; tptr < Globals.primaryBuffer.Length; ++tptr)
            {
                CharInfo ci = Globals.textBuffer[tptr];
                if(ci.AsciiChar != 0x00)
                {
                    Globals.primaryBuffer[tptr].Attributes = ci.Attributes;
                    Globals.primaryBuffer[tptr].UnicodeChar = (short)0; //remove all data from the char section of the CharInfo
                    Globals.primaryBuffer[tptr].AsciiChar = ci.AsciiChar;
                }
            }
        }

        //draws a single pixel in a defined point
        public static void drawPixel(byte colour, Coord position)
        {
            int pxPositionInBuffer = (position.Y * Globals.DISP_X) + position.X;
            if (pxPositionInBuffer >= Globals.graphicsBuffer.Length) { return; }
            Globals.graphicsBuffer[pxPositionInBuffer] = colour;
        }

        //merges the colours above into a single byte
        //console colours use the first 4 bytes to define the background colour and the last 4 bytes to define the text colour
        public static byte mergeColours(byte background, byte text)
        {
            //merges two colours into one windows-recognised code by shifting the text byte 4 to the right and then ORing it with the background byte
            return (byte)((text >> 4) ^ background);
        }

        static byte extractBackground(byte attrib)
        {
            return (byte)((attrib >> 4) << 4);
        }

        static byte extractText(byte attrib)
        {
            return (byte)(attrib << 4);
        }

        //fills the display with one colour
        public static void clearDisplay(byte colourSet)
        {
            for (int i = 0; i < Globals.graphicsBuffer.Length; ++i)
            {
                Globals.graphicsBuffer[i] = colourSet;
            }
        }

        //draws lines using bresenhams algorithm
        //https://stackoverflow.com/questions/11678693/all-cases-covered-bresenhams-line-algorithm
        public static void drawLine(byte color, Coord p1, Coord p2)
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
                drawPixel(color, new Coord(p1.X, p1.Y));
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
        public static void drawString(string text, Coord pos, byte colourCode, stAlignment alignment = stAlignment.LEFT, bool wrap = true)
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
                if (startPositionInBuffer + i > Globals.textBuffer.Length || startPositionInBuffer + i < 0) { return; }
                if (!wrap && (startPositionInBuffer + i) % Globals.DISP_X == 0) { return; } //imperfect when string is centered
                Globals.textBuffer[startPositionInBuffer + i].Attributes = colourCode;
                Globals.textBuffer[startPositionInBuffer + i].AsciiChar = (byte)text[i];
            }
        }

        //draws a rectangle
        public static void drawRect(Coord pos1, Coord pos2, byte colour, char fillChar)
        {
            //top line
            drawLine(colour, new Coord(pos1.X, pos1.Y), new Coord(pos2.X, pos1.Y));

            //double left lines
            drawLine(colour, new Coord(pos1.X, pos1.Y), new Coord(pos1.X, pos2.Y));

            //double right lines
            drawLine(colour, new Coord(pos2.X, pos2.Y), new Coord(pos2.X, pos1.Y));

            //bottom line
            drawLine(colour, new Coord(pos1.X, pos2.Y), new Coord(pos2.X, pos2.Y));
        }

        //pretty shit as it only compares RGB, maybe use hue?
        static byte getClosestColourByRGB(int R, int G, int B, bool forceColour = false)
        {
            winColour current = winColours.BLACK;
            int currentVariation = Int32.MaxValue;
            foreach(winColour colour in (forceColour ? winColours.LIST_NOPRIM : winColours.LIST))
            {
                int variation = (int)Math.Sqrt((R - colour.R) * (R - colour.R) + (G - colour.G) * (G - colour.G) + (B - colour.B) * (B - colour.B));
                if(variation < currentVariation)
                {
                    current = colour;
                    currentVariation = variation;
                }
            }
            return current.Code;
        }

        //this is very, very inefficient
        //ideally we'd load and resize the image once on the programs load and write to the screen from that each time
        //GetPixel should also be avoided as its very slow, im pretty sure its possible to access the data directly with GDI
        public static void drawImage(SmallRect boundry, Bitmap image, bool forceColour = false)
        {
            //resizes the image for drawing into the boundry box. messy. dirty. memory leaky. ew.
            Bitmap resize = new Bitmap(boundry.Bottom - boundry.Top, boundry.Right - boundry.Left);
            Graphics g = Graphics.FromImage(resize);
            g.DrawImage(image, 0, 0, resize.Width, resize.Height);
            Coord pos = new Coord(boundry.Left, boundry.Top);

            for (int pY = 0; pY < resize.Height; pY++)
            {
                for (int pX = 0; pX < resize.Width; pX++)
                {
                    Color pxColor = resize.GetPixel(pX, pY);
                    byte closestWincolour = getClosestColourByRGB(pxColor.R, pxColor.G, pxColor.B, forceColour);

                    int pxPositionInBuffer = ((pos.Y + pY) * Globals.DISP_X) + (pos.X + pX);
                    if (pxPositionInBuffer >= Globals.graphicsBuffer.Length) { continue; }

                    Globals.graphicsBuffer[pxPositionInBuffer] = closestWincolour;
                }
            }

            resize.Dispose();
            g.Dispose();
        }
    }
}
