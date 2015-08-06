using OpenTKEngine.Core.Scene.Cameras;
using OpenTKEngine.Core.Scene.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core.Scene
{
    public class Scene : IUpdateable
    {
        private List<SceneEntity> entities;
        private Renderer renderer;

        public Scene(Camera camera)
        {
            renderer = new Renderer(camera);
            entities = new List<SceneEntity>();
        }

        public void AddEntity(SceneEntity newEntity)
        {
            entities.Add(newEntity);
        }

        public void AddCamera(Camera camera)
        {
            renderer.AddCamera(camera);
        }

        public void AddLight(Light light)
        {
            renderer.AddLight(light);
        }

        public void Draw()
        {
            foreach (var entity in entities)
            {
                renderer.Render(entity);
            }
        }

        public void Update()
        {
            renderer.Update();

            foreach (var entity in entities)
            {
                entity.Update();
            }
        }

        public void OnResize(int width, int height)
        {
            if (renderer.MainCamera != null)
            {
                if (renderer.MainCamera is PerspectiveCamera)
                {
                    (renderer.MainCamera as PerspectiveCamera).AspectRatio = (float)width / (float)height;
                }
            }

            //for (int i = 0; i < cameras.Count; i++)
            //{
            //    var perspCam = cameras[i] as PerspectiveCamera;
            //    //var orthoCam = cameras[i] as OrthographicCamera;
            //    if (perspCam != null)
            //    {
            //        perspCam.AspectRatio = (float)width / (float)height;
            //    }
            //    //else if(orthoCam != null)
            //    //{

            //    //}
            //}
        }
    }
}
