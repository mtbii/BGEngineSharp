using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using OpenTKEngine.Core.Scene.Material;

namespace OpenTKEngine.Core
{
    public class EntityBuffer : IDisposable
    {
        private int vboId;
        private int iboId;
        private int count;

        public EntityBuffer()
        {
            vboId = 0;
            iboId = 0;
            count = 0;

            vboId = GL.GenBuffer();
            iboId = GL.GenBuffer();
        }

        public void SetVertices(IEnumerable<Vertex> vertices, IEnumerable<ushort> indices)
        {
            ushort[] indexArray = indices.ToArray();
            count = indexArray.Length;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexArray.Length * sizeof(ushort)), indexArray, BufferUsageHint.StaticDraw);

            Vertex[] vertexArray = vertices.ToArray();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArray.Length * Marshal.SizeOf(typeof(Vertex))), vertexArray, BufferUsageHint.StaticDraw);
        }

        public void Draw()
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, iboId);

            //Position attribute pointer
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "position"));

            //Texture coordinate attribute pointer
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, true, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "texCoord"));

            //Normal coordinate attribute pointer
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, Marshal.SizeOf(typeof(Vertex)), Marshal.OffsetOf(typeof(Vertex), "normal"));

            GL.DrawElements(BeginMode.Triangles, count, DrawElementsType.UnsignedShort, 0);
        }

        public void Dispose()
        {
            if (vboId != 0)
            {
                GL.DeleteBuffers(1, ref vboId);
                vboId = 0;
            }

            if (iboId != 0)
            {
                GL.DeleteBuffers(1, ref iboId);
                iboId = 0;
            }
        }
    }
}
