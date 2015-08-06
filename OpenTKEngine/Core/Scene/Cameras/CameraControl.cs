using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTKEngine.Core.Scene.Cameras
{
    public class CameraControl
    {
        public delegate void CameraControlCallback(TargetCamera camera, MouseState previousMouseState, KeyboardState previousKeyboardState);
        private CameraControlCallback UpdateFunc;

        private MouseState oldMouseState;
        private KeyboardState oldKeyboardState;

        public CameraControl(CameraControlType type = CameraControlType.ArcBall)
        {
            oldMouseState = Mouse.GetState();
            oldKeyboardState = Keyboard.GetState();

            switch (type)
            {
                case CameraControlType.ArcBall:
                    UpdateFunc = (camera, mouse, keyboard) =>
                    {
                        ArcBall(camera);
                    };
                    break;

                case CameraControlType.FPS:
                    UpdateFunc = (camera, mouse, keyboard) =>
                    {
                        FPS(camera);
                    };
                    break;
            }
        }

        public CameraControl(CameraControlCallback callback)
        {
            UpdateFunc = callback;
        }

        public void Update(TargetCamera camera)
        {
            if (UpdateFunc != null)
            {
                UpdateFunc(camera, this.oldMouseState, this.oldKeyboardState);
            }
        }

        private void ArcBall(TargetCamera camera)
        {
            var position = camera.Position;
            var target = camera.Target;
            var newMouseState = Mouse.GetState();
            var newKeyboardState = Keyboard.GetState();

            var mouseDelta = new Vector2(newMouseState.X - oldMouseState.X, newMouseState.Y - oldMouseState.Y);
            Vector3 lookAtVector = position - target;
            var up = Vector3.UnitY;

            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                Vector3 screenXAxis = Vector3.Cross(lookAtVector, up).Normalized();
                Vector3 screenYAxis = Vector3.Cross(screenXAxis, lookAtVector).Normalized();

                var moveDelta = 0.1f * (mouseDelta.X * screenXAxis + mouseDelta.Y * screenYAxis);
                var directionVector = (lookAtVector + moveDelta).Normalized();

                if (newKeyboardState.IsKeyDown(Key.ShiftLeft))
                {
                    directionVector *= lookAtVector.Length;
                    position = target + directionVector;
                    target += moveDelta;
                }
                else
                {
                    directionVector *= lookAtVector.Length;
                    position = target + directionVector;
                }
            }

            if (newKeyboardState.IsKeyDown(Key.PageDown))
            {
                if (!oldKeyboardState.IsKeyDown(Key.PageDown))
                {
                    lookAtVector *= 1.1f;

                    position = target + lookAtVector;

                }
            }

            if (newKeyboardState.IsKeyDown(Key.PageUp))
            {
                if (!oldKeyboardState.IsKeyDown(Key.PageUp))
                {
                    var wheelDelta = newMouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue;
                    lookAtVector *= (1.0f / 1.1f);

                    position = target + lookAtVector;
                }
            }

            oldMouseState = newMouseState;
            oldKeyboardState = newKeyboardState;

            camera.Position = position;
            camera.Target = target;
        }

        private void FPS(TargetCamera camera)
        {
            var position = camera.Position;
            var target = camera.Target;
            var newMouseState = Mouse.GetState();
            var newKeyboardState = Keyboard.GetState();
            var bounds = Game.Current.Bounds;

            var directionVector = (target - position).Normalized();

            float halfScreen = (bounds.Top - bounds.Bottom) / 2;
            float angleScreenRatio = 45.0f / halfScreen;

            var mouseDelta = new Vector2(newMouseState.X - oldMouseState.X, newMouseState.Y - oldMouseState.Y);
            var angleDelta = angleScreenRatio * mouseDelta;
            var up = Vector3.UnitY;

            Mouse.SetPosition(0.5 * (bounds.Left + bounds.Right), 0.5 * (bounds.Top + bounds.Bottom));

            oldMouseState = newMouseState;
            oldKeyboardState = newKeyboardState;

            camera.Position = position;
            camera.Target = target;
        }
    }
}
