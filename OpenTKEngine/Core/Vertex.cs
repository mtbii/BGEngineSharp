using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKEngine.Core
{
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Position3
    {
        float x;
        float y;
        float z;

        public Position3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Position2
    {
        float x;
        float y;

        public Position2(float _x, float _y)
        {
            x = _x;
            y = _y;
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Vertex
    {
        public Position3 position;
        public Position2 texCoord;
        public Position3 normal;

        public Vertex(Position3 pos, Position2 tex, Position3 norm)
        {
            position = pos;
            texCoord = tex;
            normal = norm;
        }
    }
}
