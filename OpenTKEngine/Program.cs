using OpenTKEngine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenTKEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game1 = new Game();

            game1.X = (Screen.PrimaryScreen.Bounds.Width - game1.Width) / 2;
            game1.Y = (Screen.PrimaryScreen.Bounds.Height - game1.Height) / 2;

            game1.Run(60.0);
            game1.Dispose();
        }
    }
}
