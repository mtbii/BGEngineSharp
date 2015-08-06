using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTKEngine.Core.Scene.Material;
using System.Drawing;
using OpenTK;

namespace OpenTKEngine.Core.Scene
{
    public class SceneEntity : ISceneNode
    {
        public Transform Transform { get; set; }
        public Mesh Mesh { get; set; }
        public BaseMaterial Material { get; set; }
        
        public SceneEntity Parent { get; set; }
        public List<SceneEntity> Children { get; set; }

        public SceneEntity()
        {
            Children = new List<SceneEntity>();
            Transform = new Transform();
        }

        public SceneEntity(Mesh mesh)
        {
            Children = new List<SceneEntity>();
            Mesh = mesh;
            Transform = new Transform();
        }

        public SceneEntity(Mesh mesh, Transform transform)
        {
            Children = new List<SceneEntity>();
            Mesh = mesh;
            Transform = transform;
        }

        public SceneEntity(Mesh mesh, Transform transform, BaseMaterial material)
        {
            Children = new List<SceneEntity>();
            Mesh = mesh;
            Transform = transform;
            Material = material;
        }

        private Matrix4 GetWorldTransform()
        {
            if (Parent == null)
            {
                return Transform.TransformationMatrix;
            }
            return Parent.GetWorldTransform() * Transform.TransformationMatrix;
        }

        public void Update()
        {
        }

        public void UpdateAll()
        {
            foreach (var entity in Children)
            {
                entity.UpdateAll();
            }
        }

        public void DrawAll(Shader shader, Renderer renderer)
        {
            Draw(shader, renderer);

            foreach (var entity in Children)
            {
                entity.DrawAll(shader, renderer);
            }
        }

        public void DrawAll(Renderer renderer)
        {
            Draw(renderer);

            foreach (var entity in Children)
            {
                entity.DrawAll(renderer);
            }
        }

        public void Draw(Shader shader, Renderer renderer)
        {
            Matrix4 worldTransform = Matrix4.Identity;
            if (Parent != null)
            {
                worldTransform = Parent.GetWorldTransform();
            }
            Matrix4 modelMatrix = worldTransform * Transform.TransformationMatrix;
            shader.Bind();
            shader.SetUniforms(renderer.MainCamera, renderer.MainLight, modelMatrix, Material);
            Mesh.Draw();
        }

        public void Draw(Renderer renderer)
        {
            Matrix4 worldTransform = Matrix4.Identity;
            if (Parent != null)
            {
                worldTransform = Parent.GetWorldTransform();
            }
            Matrix4 modelMatrix = worldTransform * Transform.TransformationMatrix;

            /*TODO: Finish ambient shader*/
            Shader.BasicShader.Bind();
            Shader.BasicShader.SetUniforms(renderer.MainCamera, renderer.MainLight, modelMatrix, Material);
            Mesh.Draw();
        }
    }
}
