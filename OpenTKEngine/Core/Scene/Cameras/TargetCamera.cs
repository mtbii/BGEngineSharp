using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;

namespace OpenTKEngine.Core.Scene.Cameras
{

    public class TargetCamera : PerspectiveCamera
    {
        public TargetCamera(Vector3 position, Vector3 target, float fov, float aspectRatio, float zNear, float zFar, CameraControlType type = CameraControlType.ArcBall)
            : base(fov, aspectRatio, zNear, zFar)
        {
            this.Position = position;
            this.Target = target;
            this.ViewMatrix = Matrix4.LookAt(position, target, Vector3.UnitY);
            this.ControlScheme = new CameraControl(type);
        }

        public override void Update()
        {
            base.Update();
            ControlScheme.Update(this);
            this.ViewMatrix = Matrix4.LookAt(Position, Target, Vector3.UnitY);
        }

        public CameraControl ControlScheme { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Target { get; set; }
    }
}
