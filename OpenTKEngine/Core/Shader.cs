using OpenTK;
using OpenTK.Graphics.OpenGL4;
using OpenTKEngine.Core.Scene;
using OpenTKEngine.Core.Scene.Cameras;
using OpenTKEngine.Core.Scene.Lights;
using OpenTKEngine.Core.Scene.Material;
using OpenTKEngine.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core
{
    public class Shader : IDisposable
    {
        private static Shader _basicShader;
        private static Shader _ambientShader;
        private static Shader _phongShader;

        private Dictionary<string, int> uniforms;
        private int programId;
        private int vertexShaderId;
        private int fragmentShaderId;
        private int numAttributes;

        public Shader(string vertexShader, string fragmentShader)
        {
            uniforms = new Dictionary<string, int>();

            vertexShaderId = GL.CreateShader(ShaderType.VertexShader);
            if (vertexShaderId == 0)
            {
                Log.Error("Could not create vertex shader");
            }

            fragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);
            if (fragmentShaderId == 0)
            {
                Log.Error("Could not create fragment shader");
            }

            Compile(vertexShader, vertexShaderId);
            Compile(fragmentShader, fragmentShaderId);
            programId = GL.CreateProgram();
        }

        private void Compile(string shaderSource, int id)
        {
            GL.ShaderSource(id, shaderSource);
            GL.CompileShader(id);

            int success = 0;
            GL.GetShader(id, ShaderParameter.CompileStatus, out success);
            if (Convert.ToBoolean(success) == false)
            {
                string info;
                GL.GetShaderInfoLog(id, out info);
                GL.DeleteShader(id);
                Log.Error(info);
                Log.Fatal("Could not compile shader from file");
            }
        }

        public void Link()
        {
            GL.AttachShader(programId, vertexShaderId);
            GL.AttachShader(programId, fragmentShaderId);
            GL.LinkProgram(programId);

            int success = 0;
            GL.GetProgram(programId, GetProgramParameterName.LinkStatus, out success);
            if (Convert.ToBoolean(success) == false)
            {
                string info;
                GL.GetProgramInfoLog(programId, out info);
                GL.DeleteProgram(programId);
                GL.DeleteShader(vertexShaderId);
                GL.DeleteShader(fragmentShaderId);

                Log.Error(info);
                Log.Fatal("Could not link shaders");
            }

            GL.DetachShader(programId, vertexShaderId);
            GL.DetachShader(programId, fragmentShaderId);
            GL.DeleteShader(vertexShaderId);
            GL.DeleteShader(fragmentShaderId);
        }

        public void AddAttribute(string attribName)
        {
            GL.BindAttribLocation(programId, numAttributes++, attribName);
        }

        public void AddUniform(string uniformName)
        {
            int uniformLocation = GL.GetUniformLocation(programId, uniformName);
            if (uniformLocation == -1)
            {
                Log.Error("Error: Could not find uniform: " + uniformName);
            }

            if (!uniforms.ContainsKey(uniformName))
            {
                uniforms.Add(uniformName, uniformLocation);
            }
        }

        public void SetUniforms(Camera camera, Light light, Matrix4 model, BaseMaterial material)
        {
            //Matrix uniforms
            SetUniform("mvpMat", model * camera.ViewMatrix * camera.ProjectionMatrix);
            SetUniform("mvMat", model * camera.ViewMatrix);
            SetUniform("normalMat", Matrix4.Transpose(Matrix4.Invert(model)));

            //Light uniforms
            if (light != null)
            {
                if (light is PointLight)
                {
                    SetUniform("light.position", ((PointLight)light).Position);
                }

                SetUniform("light.diffuse", light.Diffuse);
                SetUniform("light.ambient", light.Ambient);
                SetUniform("light.specular", light.Specular);
            }

            //Material uniforms
            SetUniform("material.diffuse", material.Diffuse);
            SetUniform("material.ambient", material.Ambient);
            SetUniform("material.specular", material.Specular);
            SetUniform("material.emissive", material.Emissive);
        }

        public void SetUniform(string uniformName, int value)
        {
            if (uniforms.ContainsKey(uniformName))
            {
                GL.Uniform1(uniforms[uniformName], value);
            }
            else
            {
                Log.Error("Could not set uniform: " + uniformName + ". Uniform not added.");
            }
        }

        public void SetUniform(string uniformName, float value)
        {
            if (uniforms.ContainsKey(uniformName))
            {
                GL.Uniform1(uniforms[uniformName], value);
            }
            else
            {
                Log.Error("Could not set uniform: " + uniformName + ". Uniform not added.");
            }
        }

        public void SetUniform(string uniformName, System.Drawing.Color color)
        {
            SetUniform(uniformName, new Vector4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f));
        }

        public void SetUniform(string uniformName, Vector2 value)
        {
            if (uniforms.ContainsKey(uniformName))
            {
                GL.Uniform2(uniforms[uniformName], value);
            }
            else
            {
                Log.Error("Could not set uniform: " + uniformName + ". Uniform not added.");
            }
        }

        public void SetUniform(string uniformName, Vector3 value)
        {
            if (uniforms.ContainsKey(uniformName))
            {
                GL.Uniform3(uniforms[uniformName], value);
            }
            else
            {
                Log.Error("Could not set uniform: " + uniformName + ". Uniform not added.");
            }
        }

        public void SetUniform(string uniformName, Vector4 value)
        {
            if (uniforms.ContainsKey(uniformName))
            {
                GL.Uniform4(uniforms[uniformName], value);
            }
            else
            {
                Log.Error("Could not set uniform: " + uniformName + ". Uniform not added.");
            }
        }

        public void SetUniform(string uniformName, Matrix4 value)
        {
            if (uniforms.ContainsKey(uniformName))
            {
                GL.UniformMatrix4(uniforms[uniformName], false, ref value);
            }
            else
            {
                Log.Error("Could not set uniform: " + uniformName + ". Uniform not added.");
            }
        }

        public void Bind()
        {
            if (programId != 0)
            {
                GL.UseProgram(programId);
                for (int i = 0; i < numAttributes; i++)
                {
                    GL.EnableVertexAttribArray(i);
                }
            }
        }

        public void Unbind()
        {
            GL.UseProgram(0);
            for (int i = 0; i < numAttributes; i++)
            {
                GL.DisableVertexAttribArray(i);
            }
        }

        public void Dispose()
        {
            if (vertexShaderId != 0)
            {
                GL.DeleteShader(vertexShaderId);
                vertexShaderId = 0;
            }
            if (fragmentShaderId != 0)
            {
                GL.DeleteShader(fragmentShaderId);
                fragmentShaderId = 0;
            }
            if (programId != 0)
            {
                GL.DeleteProgram(programId);
                programId = 0;
            }
        }

        public static Shader FromFiles(string vertexShaderFilePath, string fragmentShaderFilePath)
        {
            string vShader = "";
            string fShader = "";

            if (File.Exists(vertexShaderFilePath))
            {
                using (StreamReader reader = File.OpenText(vertexShaderFilePath))
                {
                    vShader = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(vShader))
                    {
                        Log.Error("Vertex shader file is empty: " + vertexShaderFilePath);
                    }
                }
            }
            else
            {
                Log.Fatal("Cannot find vertex shader file: " + vertexShaderFilePath);
            }

            if (File.Exists(fragmentShaderFilePath))
            {
                using (StreamReader reader = File.OpenText(fragmentShaderFilePath))
                {
                    fShader = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(fShader))
                    {
                        Log.Error("Fragment shader file is empty: " + fragmentShaderFilePath);
                    }
                }
            }
            else
            {
                Log.Fatal("Cannot find fragment shader file: " + fragmentShaderFilePath);
            }

            return new Shader(vShader, fShader);
        }

        public static Shader BasicShader
        {
            get
            {
                if (_basicShader == null)
                {
                    _basicShader = Shader.FromFiles("Resources/Shaders/basic.vert", "Resources/Shaders/basic.frag");
                    _basicShader.AddAttribute("vertexPosition");
                    _basicShader.AddAttribute("vertexTexCoord");
                    _basicShader.AddAttribute("vertexNormal");
                    _basicShader.Link();

                    //Matrix uniforms
                    _basicShader.AddUniform("mvpMat");
                    _basicShader.AddUniform("mvMat");
                    _basicShader.AddUniform("normalMat");

                    //Light uniforms
                    _basicShader.AddUniform("light.position");
                    _basicShader.AddUniform("light.diffuse");
                    _basicShader.AddUniform("light.ambient");
                    _basicShader.AddUniform("light.specular");

                    //Material uniforms
                    _basicShader.AddUniform("material.diffuse");
                    _basicShader.AddUniform("material.ambient");
                    _basicShader.AddUniform("material.specular");
                    _basicShader.AddUniform("material.emissive");
                }

                return _basicShader;
            }
        }

        public static Shader PhongShader
        {
            get
            {
                if (_phongShader == null)
                {
                    _phongShader = Shader.FromFiles("Resources/Shaders/basic.vert", "Resources/Shaders/phong.frag");
                    _phongShader.AddAttribute("vertexPosition");
                    _phongShader.AddAttribute("vertexTexCoord");
                    _phongShader.AddAttribute("vertexNormal");
                    _phongShader.Link();

                    //Matrix uniforms
                    _phongShader.AddUniform("mvpMat");
                    _phongShader.AddUniform("mvMat");
                    _phongShader.AddUniform("normalMat");

                    //Light uniforms
                    _phongShader.AddUniform("light.position");
                    _phongShader.AddUniform("light.diffuse");
                    _phongShader.AddUniform("light.ambient");
                    _phongShader.AddUniform("light.specular");

                    //Material uniforms
                    _phongShader.AddUniform("material.diffuse");
                    _phongShader.AddUniform("material.ambient");
                    _phongShader.AddUniform("material.specular");
                    _phongShader.AddUniform("material.emissive");
                }

                return _phongShader;
            }
        }

        //public static Shader AmbientShader
        //{
        //    get
        //    {
        //        if (_ambientShader == null)
        //        {
        //            _ambientShader = Shader.FromFiles("Resources/Shaders/basic.vert", "Resources/Shaders/ambient.frag");
        //            _ambientShader.AddAttribute("vertexPosition");
        //            _ambientShader.AddAttribute("vertexTexCoord");
        //            _ambientShader.AddAttribute("vertexNormal");
        //            _ambientShader.Link();

        //            //Matrix uniforms
        //            _ambientShader.AddUniform("mvpMat");
        //            _ambientShader.AddUniform("mvMat");
        //            _ambientShader.AddUniform("normalMat");

        //            //Light uniforms
        //            _ambientShader.AddUniform("light.position");
        //            _ambientShader.AddUniform("light.diffuse");
        //            _ambientShader.AddUniform("light.ambient");
        //            _ambientShader.AddUniform("light.specular");

        //            //Material uniforms
        //            _ambientShader.AddUniform("material.diffuse");
        //            _ambientShader.AddUniform("material.ambient");
        //            _ambientShader.AddUniform("material.specular");
        //            _ambientShader.AddUniform("material.emissive");
        //        }

        //        return _ambientShader;
        //    }
        //}
    }
}
