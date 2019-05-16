using Microsoft.Xna.Framework;
using PotatoChipMine.Core.GameEngine;
using System;
using System.Linq;

namespace PotatoChipMineMono.Consoles
{
    public static class PcmColorExtensions
    {
        public static Color ToColor(this PcmColor pcmColor)
        {
            return new Color(pcmColor.R, pcmColor.G, pcmColor.B, pcmColor.A);
        }
    }
}
