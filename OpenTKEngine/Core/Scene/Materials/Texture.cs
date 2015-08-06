using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevIL;
using OpenTKEngine.Core.Utilities;
using System.IO;

namespace OpenTKEngine.Core.Scene.Materials
{
    public class Texture : IDisposable
    {
        private int texId;

        public Texture()
        {
            texId = GL.GenTexture();
        }

        public Texture(string path)
            : this()
        {
            System.Drawing.Bitmap bmp = null;
            bmp = DevIL.DevIL.LoadBitmap(path);
            GL.BindTexture(TextureTarget.Texture2D, Id);

            // We will not upload mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // We can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            var bmp_data = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, texId);
        }

        public void Unbind()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public int Id
        {
            get { return texId; }
            set { texId = value; }
        }

        public void Dispose()
        {
            if (texId != 0)
            {
                GL.DeleteTexture(texId);
                texId = 0;
            }
        }
    }
}
