using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace OpenTKEngine.Core.Scene.Material
{
    public abstract class BaseMaterial : IUpdateable, IDisposable
    {
        public Color Diffuse { get; set; }
        public Color Ambient { get; set; }
        public Color Specular { get; set; }
        public Color Emissive { get; set; }
        public float Shininess { get; set; }

        public Shader Shader { get; set; }

        public virtual void Bind()
        {
            Shader.Bind();
            Update();
        }

        public virtual void Unbind()
        {
            Shader.Unbind();
        }

        public virtual void SetMVPMatrix(Matrix4 m, Matrix4 v, Matrix4 p)
        {
            Shader.SetUniform("mvpMat", m * v * p);
            Shader.SetUniform("mvMat", m * v);
            Shader.SetUniform("normalMat", Matrix4.Transpose(Matrix4.Invert(m)));
        }

        public abstract void Update();

        public abstract void Dispose();
    }
}
