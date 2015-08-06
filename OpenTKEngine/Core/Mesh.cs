using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Assimp.Configs;
using OpenTKEngine.Core.Utilities;
using OpenTKEngine.Core.Scene.Material;
using OpenTKEngine.Core.Scene.Materials;
using System.IO;
using OpenTK;

namespace OpenTKEngine.Core
{
    public class Mesh
    {
        private EntityBuffer buffer;

        public Mesh(EntityBuffer buffer)
        {
            this.buffer = buffer;
        }

        public void Draw()
        {
            buffer.Draw();
        }
    }
}
