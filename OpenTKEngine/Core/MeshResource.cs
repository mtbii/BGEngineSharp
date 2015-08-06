using Assimp;
using Assimp.Configs;
using OpenTKEngine.Core.Scene;
using OpenTKEngine.Core.Scene.Material;
using OpenTKEngine.Core.Scene.Materials;
using OpenTKEngine.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core
{
    public class MeshResource
    {
        public Dictionary<int, Mesh> Meshes { get; set; }
        public List<BaseMaterial> Materials { get; set; }

        public MeshResource()
        {
            Meshes = new Dictionary<int, Mesh>();
            Materials = new List<BaseMaterial>();
        }

        public List<SceneEntity> ToSceneEntities()
        {
            List<SceneEntity> entitites = new List<SceneEntity>();

            foreach (var res in Meshes)
            {
                var mat = Materials[res.Key];
                var entity = new SceneEntity(res.Value, new Transform(), mat);
                entitites.Add(entity);
            }

            return entitites;
        }

        public static MeshResource Load(string filePath)
        {
            AssimpImporter importer = new AssimpImporter();
            importer.SetConfig(new NormalizeVertexComponentsConfig(true));
            importer.SetConfig(new MultithreadingConfig(-1));

            LogStream logStream = new LogStream(delegate(String msg, String userdata)
            {
                Log.Message(msg);
                Log.Message(userdata);
            });
            importer.AttachLogStream(logStream);

            Assimp.Scene model = importer.ImportFile(filePath, PostProcessPreset.TargetRealTimeMaximumQuality | PostProcessSteps.FlipUVs);
            MeshResource meshResource = new MeshResource();
            meshResource.Materials = model.Materials.Select(m => (BaseMaterial)new TexturedMaterial(m, Directory.GetParent(filePath).FullName)).ToList();

            foreach (var modelMesh in model.Meshes)
            {
                EntityBuffer buffer = new EntityBuffer();
                List<ushort> indices = new List<ushort>();
                List<Vertex> vertices = new List<Vertex>();
                Vector3D[] texCoords = modelMesh.HasTextureCoords(0) ? modelMesh.GetTextureCoords(0) : null;

                foreach (var face in modelMesh.Faces)
                {
                    for (int i = 0; i < face.IndexCount; i++)
                    {
                        indices.Add((ushort)face.Indices[i]);
                    }
                }

                var material = model.Materials[modelMesh.MaterialIndex];
                TexturedMaterial texMat = new TexturedMaterial(material, Directory.GetParent(filePath).FullName);

                for (int i = 0; i < modelMesh.VertexCount; i++)
                {
                    var vertex = modelMesh.Vertices[i];
                    var texCoord = texCoords[i];
                    var normal = modelMesh.Normals[i];

                    vertices.Add(new Vertex(new Position3(vertex.X, vertex.Y, vertex.Z), new Position2(texCoord.X, texCoord.Y), new Position3(normal.X, normal.Y, normal.Z)));
                }

                buffer.SetVertices(vertices, indices);

                Mesh mesh = new Mesh(buffer);
                meshResource.Meshes.Add(modelMesh.MaterialIndex, mesh);
            }

            importer.Dispose();

            return meshResource;
        }

    }
}
