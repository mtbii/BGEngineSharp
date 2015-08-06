using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core.Scene.Lights
{
    public abstract class Light : IUpdateable
    {
        public Shader Shader { get; set; }
        public Color Diffuse { get; set; }
        public Color Ambient { get; set; }
        public Color Specular { get; set; }

        public abstract void Update();
    }
}
