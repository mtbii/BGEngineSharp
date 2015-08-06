using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core.Scene.Lights
{
    public class PointLight : Light
    {
        public Vector3 Position { get; set; }

        public PointLight(Color diffuse, Vector3 position)
        {
            Diffuse = diffuse;
            Ambient = Color.FromArgb(diffuse.A, (int)(0.1 * diffuse.R), (int)(0.1 * diffuse.G), (int)(0.1 * diffuse.B));
            Specular = Color.White;
            Position = position;
            Shader = Shader.PhongShader;
        }

        public PointLight(Color diffuse, Color ambient, Color specular, Vector3 position)
        {
            Diffuse = diffuse;
            Ambient = ambient;
            Specular = specular;
            Position = position;
            Shader = Shader.PhongShader;
        }

        public override void Update()
        {
        }
    }
}
