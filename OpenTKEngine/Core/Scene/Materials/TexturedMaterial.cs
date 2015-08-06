using OpenTK.Graphics.OpenGL4;
using OpenTKEngine.Core.Scene.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKEngine.Core.Utilities;
using System.IO;

namespace OpenTKEngine.Core.Scene.Materials
{
    public class TexturedMaterial : BaseMaterial
    {
        List<Texture> textures;
        List<Assimp.TextureSlot> textureSlots;

        public TexturedMaterial()
        {
            textures = new List<Texture>();
            Shader = Shader.BasicShader;
        }

        public TexturedMaterial(Assimp.Material m, string materialDirectory)
            : this()
        {
            Diffuse = m.ColorDiffuse.ToNETColor();
            Specular = m.ColorSpecular.ToNETColor();
            //Ambient = m.ColorAmbient.ToNETColor();
            Emissive = m.ColorEmissive.ToNETColor();

            textureSlots = new List<Assimp.TextureSlot>(m.GetAllTextures());
            LoadTextures(materialDirectory);
        }

        private void LoadTextures(string materialDirectory)
        {
            foreach (var textureSlot in textureSlots)
            {
                string path = Path.Combine(materialDirectory, textureSlot.FilePath);

                try
                {
                    Texture newTexture = new Texture(path);//Texture.Load(path);
                    textures.Add(newTexture);
                }
                catch (Exception e)
                {
                    Log.Fatal(e.Message);
                }
            }
        }

        public void AddTexture(Texture t)
        {
            textures.Add(t);
        }

        public override void Bind()
        {
            for (int i = 0; i < textures.Count; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                GL.BindTexture(TextureTarget.Texture2D, textures[i].Id);
            }

            //Shader gets bound here
            base.Bind();

            //Update uniforms after shader binding
            Shader.SetUniform("diffuseColor", Diffuse);
            Shader.SetUniform("ambientColor", Ambient);
            Shader.SetUniform("specularColor", Specular);
            Shader.SetUniform("emissiveColor", Emissive);
        }

        public override void Unbind()
        {
            base.Unbind();

            for (int i = 0; i < textures.Count; i++)
            {
                GL.ActiveTexture(TextureUnit.Texture0 + i);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }

        public override void Update()
        {
        }

        public override void Dispose()
        {
            foreach (var texture in textures)
            {
                texture.Dispose();
            }
            textures.Clear();
        }
    }
}
