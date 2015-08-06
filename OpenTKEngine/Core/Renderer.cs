using OpenTKEngine.Core.Scene;
using OpenTKEngine.Core.Scene.Cameras;
using OpenTKEngine.Core.Scene.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core
{
    public class Renderer
    {
        private List<Camera> cameras;
        private List<Light> lights;

        public Renderer(Scene.Cameras.Camera camera)
        {
            cameras = new List<Camera>();
            lights = new List<Light>();
            AddCamera(camera);
            MainCamera = camera;
        }

        public void Render(SceneEntity entity)
        {
            entity.DrawAll(this);

            foreach (var light in lights)
            {
                MainLight = light;
                entity.DrawAll(light.Shader, this);
            }
        }

        internal void AddCamera(Camera camera)
        {
            cameras.Add(camera);
        }

        internal void AddLight(Light light)
        {
            lights.Add(light);
        }

        internal void Update()
        {
            foreach (var light in lights)
            {
                light.Update();
            }

            foreach (var camera in cameras)
            {
                camera.Update();
            }
        }

        public Camera MainCamera { get; set; }

        public Light MainLight { get; set; }
    }
}
