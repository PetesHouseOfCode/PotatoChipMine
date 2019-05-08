using System;

namespace PotatoChipMine.Core.GameEngine
{
    public class PcmColor
    {
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }
        public byte A { get; private set; }

        public PcmColor(PcmColor color, int alpha)
        {
            R = color.R;
            B = color.B;
            G = color.G;
            A = Convert.ToByte(alpha);
        }
        public PcmColor(PcmColor color, float alpha)
        {
            R = color.R;
            B = color.B;
            G = color.G;
            A = Convert.ToByte(alpha);
        }
        public PcmColor(float r, float g, float b)
        {
            R = Convert.ToByte(r);
            B = Convert.ToByte(g);
            G = Convert.ToByte(b);
            A = 255;
        }
        public PcmColor(int r, int g, int b)
        {
            R = Convert.ToByte(r);
            G = Convert.ToByte(g);
            B = Convert.ToByte(b);
            A = 255;
        }
        public PcmColor(byte r, byte g, byte b, byte alpha)
        {
            R = r;
            G = g;
            B = b;
            A = alpha;
        }
        public PcmColor(int r, int g, int b, int alpha)
        {
            R = Convert.ToByte(r);
            G = Convert.ToByte(g);
            B = Convert.ToByte(b);
            A = Convert.ToByte(alpha);
        }
        public PcmColor(float r, float g, float b, float alpha)
        {
            R = Convert.ToByte(r);
            G = Convert.ToByte(g);
            B = Convert.ToByte(b);
            A = Convert.ToByte(alpha);
        }

        public static readonly PcmColor Black = new PcmColor(0, 0, 0);
        public static readonly PcmColor White = new PcmColor(255, 255, 255);
        public static readonly PcmColor Red = new PcmColor(255, 0, 0);
        public static readonly PcmColor Green = new PcmColor(0, 255, 0);
        public static readonly PcmColor DarkGreen = new PcmColor(0, 102, 0);
        public static readonly PcmColor Blue = new PcmColor(0, 0, 255);
        public static readonly PcmColor Cyan = new PcmColor(0, 255, 255);
        public static readonly PcmColor Yellow = new PcmColor(255, 255, 0);
        public static readonly PcmColor DarkYellow = new PcmColor(204, 204, 0);
    }
}
