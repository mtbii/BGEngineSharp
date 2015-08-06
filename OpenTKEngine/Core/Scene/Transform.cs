using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace OpenTKEngine.Core.Scene
{
    public class Transform
    {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;

        private Matrix4 positionMatrix;
        private Matrix4 rotationMatrix;
        private Matrix4 scaleMatrix;

        private Matrix4 transformationMatrix;
        private bool dirtyTransformationMatrix;

        public Transform()
        {
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            Scale = Vector3.One;
        }

        public Transform(Vector3 position, Vector3 angles, Vector3 scale)
        {
            Position = position;
            RotationEuler = angles;
            Scale = scale;
        }

        public Matrix4 TransformationMatrix
        {
            get
            {
                if (dirtyTransformationMatrix)
                {
                    transformationMatrix = ScaleMatrix * RotationMatrix * PositionMatrix;
                    dirtyTransformationMatrix = false;
                }
                return transformationMatrix;
            }
            set
            {
                transformationMatrix = value;
                Rotation = transformationMatrix.ExtractRotation();
                Scale = transformationMatrix.ExtractScale();
                Position = transformationMatrix.ExtractTranslation();
            }
        }

        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                positionMatrix = Matrix4.CreateTranslation(value);
                dirtyTransformationMatrix = true;
            }
        }

        public Matrix4 PositionMatrix
        {
            get
            {
                return positionMatrix;
            }
            set
            {
                positionMatrix = value;
                position = new Vector3(Vector4.Transform(Vector4.Zero, value));
                dirtyTransformationMatrix = true;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
                rotationMatrix = Matrix4.CreateFromQuaternion(value);
                dirtyTransformationMatrix = true;
            }
        }

        public Vector3 RotationEuler
        {
            set
            {
                FromEulerAngles(ref value, out rotation);
                Rotation = rotation;
                dirtyTransformationMatrix = true;
            }
        }

        public Matrix4 RotationMatrix
        {
            get
            {
                return rotationMatrix;
            }
            set
            {
                rotationMatrix = value;
                rotation = Quaternion.FromMatrix(new Matrix3(value));
                dirtyTransformationMatrix = true;
            }
        }

        public Vector3 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                scaleMatrix = Matrix4.CreateScale(value);
                dirtyTransformationMatrix = true;
            }
        }

        public Matrix4 ScaleMatrix
        {
            get
            {
                return scaleMatrix;
            }
            set
            {
                scaleMatrix = value;
                scale = new Vector3(Vector4.Transform(Vector4.One, scaleMatrix));
                dirtyTransformationMatrix = true;
            }
        }

        public static void FromEulerAngles(ref Vector3 eulerAngles, out Quaternion result)
        {
            result = new Quaternion();

            float c1 = (float)Math.Cos(eulerAngles.Y * 0.5f);
            float c2 = (float)Math.Cos(eulerAngles.X * 0.5f);
            float c3 = (float)Math.Cos(eulerAngles.Z * 0.5f);
            float s1 = (float)Math.Sin(eulerAngles.Y * 0.5f);
            float s2 = (float)Math.Sin(eulerAngles.X * 0.5f);
            float s3 = (float)Math.Sin(eulerAngles.Z * 0.5f);

            result.W = c1 * c2 * c3 - s1 * s2 * s3;
            result.X = s1 * s2 * c3 + c1 * c2 * s3;
            result.Y = s1 * c2 * c3 + c1 * s2 * s3;
            result.Z = c1 * s2 * c3 - s1 * c2 * s3;
        }
    }
}
