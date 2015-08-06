using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core
{
    public interface ISceneNode : IRenderable, IUpdateable
    {
        void DrawAll(Shader shader, Renderer renderer);
        void DrawAll(Renderer renderer);
        void UpdateAll();
    }
}
