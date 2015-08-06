using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core.Utilities
{
    public static class Extensions
    {
        public static System.Drawing.Color ToNETColor(this Assimp.Color4D color)
        {
            return System.Drawing.Color.FromArgb((int)(color.A * 255), (int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255));
        }
    }
}
