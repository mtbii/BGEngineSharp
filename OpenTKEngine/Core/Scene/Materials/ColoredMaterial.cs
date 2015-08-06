using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OpenTKEngine.Core.Scene.Material
{
    public class ColoredMaterial : BaseMaterial
    {
        public ColoredMaterial(Color diffuse, Color specular, Color ambient)
        {
            Diffuse = diffuse;
            Specular = specular;
            Ambient = ambient;
            Emissive = Color.Black;

            Shader = Shader.BasicShader;
            Shader.SetUniform("diffuseColor", Diffuse);
        }

        public ColoredMaterial(Color diffuse)
        {
            Diffuse = diffuse;
            Specular = Color.White;
            Ambient = Color.FromArgb(diffuse.A, (int)(0.1 * diffuse.R), (int)(0.1 * diffuse.G), (int)(0.1 * diffuse.B));
            Emissive = Color.Black;

            Shader = Shader.BasicShader;
            Shader.SetUniform("diffuseColor", Diffuse);
        }

        public override void Update()
        {
            Shader.SetUniform("diffuseColor", Diffuse);
        }

        public override void Dispose()
        {

        }
    }
}
