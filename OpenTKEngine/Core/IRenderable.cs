using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core
{
    public interface IRenderable
    {
        void Draw(Shader shader, Renderer renderer);
        void Draw(Renderer renderer);
    }
}
