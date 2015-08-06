using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System.Drawing;
using OpenTKEngine.Core.Scene.Cameras;
using OpenTKEngine.Core.Scene;
using OpenTKEngine.Core.Utilities;
using OpenTKEngine.Core.Scene.Material;

namespace OpenTKEngine.Core
{
    public class Game : GameWindow
    {
        Scene.Scene scene;
        SceneEntity triangle;
        private double TotalTime;

        public static Game Current { get; set; }

        public Game(int width = 800, int height = 600, string title = "OpenTK Engine")
            : base(width, height, GraphicsMode.Default, title, GameWindowFlags.Default, DisplayDevice.Default, 4, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Log.Message("OpenGL Version: " + GL.GetInteger(GetPName.MajorVersion) + "." + GL.GetInteger(GetPName.MinorVersion));
            Log.Message("OpenGL Version: " + GL.GetString(StringName.Version));
            Log.Message("Shader Version: " + GL.GetString(StringName.ShadingLanguageVersion));

            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.FramebufferSrgb);

            //GL.Enable(EnableCap.Texture2D);

            GL.ClearColor(Color.CornflowerBlue);
            Current = this;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.VSync = VSyncMode.Adaptive;

            scene = new Scene.Scene(new TargetCamera(new Vector3(5), 2*Vector3.UnitY, (float)MathHelper.DegreesToRadians(45.0), (float)this.Width / (float)this.Height, .1f, 100.0f));

            var buffer = new EntityBuffer();
            float side = 0.4f;

            buffer.SetVertices(
                new Vertex[]{
                    new Vertex(new Position3(0, (float)(side*Math.Sqrt(2)), 0), new Position2(), new Position3()),
                    new Vertex(new Position3(side, -side, 0), new Position2(), new Position3()),
                    new Vertex(new Position3(-side, -side, 0), new Position2(), new Position3())
                },
                new ushort[]{
                   1,0,2
                });

            Mesh mesh = new Mesh(buffer);
            triangle = new SceneEntity(mesh, new Transform(-0.5f * Vector3.One, Vector3.Zero, Vector3.One), new ColoredMaterial(Color.Green));

            scene.AddEntity(triangle);
            var resource = MeshResource.Load("Resources/Models/generic_male_01/generic_male_01.obj");
            var entities = resource.ToSceneEntities();
            foreach (var entity in entities)
            {
                scene.AddEntity(entity);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, this.Width, this.Height);
            scene.OnResize(this.Width, this.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            TotalTime += e.Time;

            var keyboard = OpenTK.Input.Keyboard.GetState();

            if (keyboard.IsKeyDown(Key.Escape))
            {
                this.Exit();
            }

            float radius = 2f;
            triangle.Transform.Position = new Vector3(radius * (float)Math.Cos(2 * TotalTime), 0, radius * (float)Math.Sin(2 * TotalTime));
            triangle.Transform.Rotation = Quaternion.FromAxisAngle(Vector3.UnitX, -MathHelper.PiOver2) * Quaternion.FromAxisAngle(Vector3.UnitZ, 10 * (float)TotalTime);

            scene.Update();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            scene.Draw();

            this.SwapBuffers();
        }
    }
}
