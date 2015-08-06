using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Core.Scene.Cameras
{
    public abstract class PerspectiveCamera : Camera
    {
        private float _fov;
        private float _aspectRatio;

        public PerspectiveCamera(float fov, float aspectRatio, float zNear, float zFar)
        {
            this._fov = fov;
            this._aspectRatio = aspectRatio;
            this._zNear = zNear;
            this._zFar = zFar;
            this.ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), aspectRatio, zNear, zFar);
        }

        public override void Update()
        {
        }

        protected override void UpdateProjectionMatrix()
        {
            this.ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FOV, AspectRatio, _zNear, _zFar);
        }

        public float FOV
        {
            get
            {
                return _fov;
            }
            set
            {
                _fov = value;
                UpdateProjectionMatrix();
            }
        }

        public float AspectRatio
        {
            get
            {
                return _aspectRatio;
            }
            set
            {
                _aspectRatio = value;
                UpdateProjectionMatrix();
            }
        }
    }
}
