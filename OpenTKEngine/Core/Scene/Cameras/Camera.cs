using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core.Scene.Cameras
{
    public enum CameraControlType
    {
        ArcBall,
        FPS
    }

    public abstract class Camera : IUpdateable
    {
        protected float _zNear;
        protected float _zFar;

        public abstract void Update();
        protected abstract void UpdateProjectionMatrix();

        public float ZNear
        {
            get
            {
                return _zNear;
            }
            set
            {
                _zNear = value;
                UpdateProjectionMatrix();
            }
        }

        public float ZFar
        {
            get
            {
                return _zFar;
            }
            set
            {
                _zFar = value;
                UpdateProjectionMatrix();
            }
        }

        public Matrix4 ProjectionMatrix { get; set; }
        public Matrix4 ViewMatrix { get; set; }
    }
}
