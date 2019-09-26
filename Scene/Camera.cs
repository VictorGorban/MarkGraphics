using System;

namespace Game_Consoles
{
    [Serializable]
    public class Camera
    {
        public bool IsCentralProjection { get; set; } = true;

        public const double MIN_FOV = 60.0;
        public const double MAX_FOV = 135.0;

        public const double MIN_THETA = 1;
        public const double MAX_THETA = 179;

        public Point3DSpherical Position;
        public Point3DSpherical Target;

        public int AnglePhi { get; private set; } = 270;
        public int AngleTheta { get; private set; } = 90;


        /**
     * Modification matrix of camera, that allows to convert points from world's
     * coordinates to camera's.
     */
        private Matrix cameraMatrix = ModificationMatrix.NewIdentityMatrix(4);

        /**
     * Field of view, angle of view.
     */
        private double fov = 90;
        public double FOV
        {
            get
            {
                return fov;
            }
            set
            {
                fov = Math.Min(Math.Max(value, MIN_FOV), MAX_FOV);
            }
        }

        /// <summary>
        /// Clipping
        /// </summary>
        public double NearClipZ { get; set; }

        /// <summary>
        /// Clipping
        /// </summary>
        public double FarClipZ { get; set; }


        /// <summary>
        /// Screen / frame / camera viewport width
        /// </summary>
        public int Width { get; private set; }
        public int HalfWidth { get; private set; }
        /// <summary>
        /// Screen / frame / camera viewport height
        /// </summary>
        public int Height { get; private set; }
        public int HalfHeight { get; private set; }

        /// <summary>
        /// A get - helper
        /// </summary>
        public double FocusDistance { get { return Width / 2.0 * Math.Tan(MathHelpers.FromDegreesToRadians(fov / 2)); } }

        public Camera()
        {

        }

        public Camera(
            Point3D position, Point3D target,
            double nearClippingPaneZ, double farClippingPaneZ,
            int frameWidth, int frameHeight)
        {
            Position = new Point3DSpherical(position);
            Target = new Point3DSpherical(target);
            NearClipZ = nearClippingPaneZ;
            FarClipZ = farClippingPaneZ;
            setSize(frameWidth, frameHeight);
        }

        public void setSize(int width, int height)
        {
            Width = width;
            HalfWidth = Width / 2;
            Height = height;
            HalfHeight = Height / 2;
        }

        /// <summary>
		/// This method calculates modification matrix, that allows to convert world
		/// coordinates to camera.
		/// </summary>
        public Matrix GetCameraMatrix()
        {
            //Stage 1: Creating inverted matrix of translation

            Matrix matrixInverted = ModificationMatrix.NewMoveMatrix(-Position.X, -Position.Y, -Position.Z);
            //Stage 2: Calculating vectors u, v, n

            Vector3D N = new Vector3D(Target.X - Position.X, Target.Y - Position.Y, Target.Z - Position.Z);
            Vector3D V = new Vector3D(0, 1, 0);
            Vector3D U = V ^ N;
            V = U ^ N;
            //Normalizing all vectors

            U.Normalize();
            V.Normalize();
            N.Normalize();

            cameraMatrix = ModificationMatrix.NewUVNProjectionMatrix(
                U.X, U.Y, U.Z, 
                V.X, V.Y, V.Z,
                N.X, N.Y, N.Z);
            cameraMatrix = cameraMatrix.Multiply(matrixInverted);

            return cameraMatrix;
        }

        /// <summary>
		/// Rotates camera up & down with @angle
		/// </summary>
        public void FlyCameraUpDown(double angle)
        {
            Matrix rotateMatrix = ModificationMatrix.NewMoveMatrix(-Target.X, -Target.Y, -Target.Z);
            rotateMatrix = rotateMatrix.Multiply(Position.GetProjectiveCoordinates());
            Position.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);

            angle += MathHelpers.FromRadiansToDegrees(Position.Theta);
            angle = Math.Min(Math.Max(angle, MIN_THETA), MAX_THETA);
            Position.SetTheta(MathHelpers.FromDegreesToRadians(angle));
            AngleTheta = 180 - (int)angle;

            rotateMatrix = ModificationMatrix.NewMoveMatrix(Target.X, Target.Y, Target.Z);
            rotateMatrix = rotateMatrix.Multiply(Position.GetProjectiveCoordinates());
            Position.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);
        }

        /// <summary>
		/// Rotates camera left & right with @angle
		/// </summary>
        public void FlyCameraLeftRight(double angle)
        {
            Matrix rotateMatrix = ModificationMatrix.NewMoveMatrix(-Target.X, -Target.Y, -Target.Z);
            rotateMatrix = rotateMatrix.Multiply(Position.GetProjectiveCoordinates());
            Position.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);

            angle += MathHelpers.FromRadiansToDegrees(Position.Phi);
            angle %= 360;
            angle = angle < 0 ? 360 + angle : angle;

            Position.SetPhi(MathHelpers.FromDegreesToRadians(angle));
            AnglePhi = (int)angle;

            rotateMatrix = ModificationMatrix.NewMoveMatrix(Target.X, Target.Y, Target.Z);
            rotateMatrix = rotateMatrix.Multiply(Position.GetProjectiveCoordinates());
            Position.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);
        }

        // do you really need any comments here?
        public void HeadCameraUpDown(double angle)
        {
            angle = MathHelpers.FromDegreesToRadians(angle);

            Matrix rotateMatrix = ModificationMatrix.NewMoveMatrix(-Position.X, -Position.Y, -Position.Z);
            rotateMatrix = rotateMatrix.Multiply(Target.GetProjectiveCoordinates());
            Target.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);

            angle += MathHelpers.FromRadiansToDegrees(Target.Theta);
            angle = Math.Min(Math.Max(angle, MIN_THETA), MAX_THETA);
            Target.SetTheta(MathHelpers.FromDegreesToRadians(angle));
            AngleTheta = 180 - (int)angle;

            rotateMatrix = ModificationMatrix.NewMoveMatrix(Position.X, Position.Y, Position.Z);
            rotateMatrix = rotateMatrix.Multiply(Target.GetProjectiveCoordinates());
            Target.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);
        }

        // do you really need any comments here?
        public void HeadCameraLeftRight(double angle)
        {
            Matrix rotateMatrix = ModificationMatrix.NewMoveMatrix(-Position.X, -Position.Y, -Position.Z);
            rotateMatrix = rotateMatrix.Multiply(Target.GetProjectiveCoordinates());
            Target.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);

            angle += MathHelpers.FromRadiansToDegrees(Target.Phi);
            angle %= 360;
            angle = angle < 0 ? 360 + angle : angle;

            Target.SetPhi(MathHelpers.FromDegreesToRadians(angle));
            AnglePhi = (int)angle;

            rotateMatrix = ModificationMatrix.NewMoveMatrix(Position.X, Position.Y, Position.Z);
            rotateMatrix = rotateMatrix.Multiply(Target.GetProjectiveCoordinates());
            Target.Set(rotateMatrix[0, 0], rotateMatrix[1, 0], rotateMatrix[2, 0]);
        }

        public void MoveCameraLeftRight(double distance)
        {
            Vector3D vector = new Vector3D(Target) - new Vector3D(Position);
            vector.Normalize();
            vector = vector ^ new Vector3D(0, 1, 0);
            vector = vector * distance;
            Target.Add(vector.X, vector.Y, vector.Z);
            Position.Add(vector.X, vector.Y, vector.Z);
        }
        public void MoveCameraUpDown(double distance)
        {
            Vector3D vector = new Vector3D(Target) - new Vector3D(Position);
            vector.Normalize();
            vector = vector ^ (vector ^ new Vector3D(0, 1, 0));
            vector = vector * distance;
            Target.Add(vector.X, vector.Y, vector.Z);
            Position.Add(vector.X, vector.Y, vector.Z);
        }

        public void SetPositionAndTarget(Point3D position, Point3D target)
        {
            this.Position.Set(position);
            this.Target.Set(target);
        }
    }
}
