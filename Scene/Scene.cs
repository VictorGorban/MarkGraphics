using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Game_Consoles
{
    [Serializable]
    public class Scene
    {
        private PictureBox canvas;

        public enum MODE
        {
            WIREFRAME = 0,
            SOLID = 1,
        }

        public RadioObject SelectedForPanObject = null;
        public bool IsPanning { get; set; }


        public Vector3D Light { get; set; }
        public Camera Camera { get; set; }
        public MODE Mode { get; set; } = MODE.WIREFRAME;
        public List<RadioObject> Objects { get; private set; } = new List<RadioObject>();

        public Scene() { }

        /// <summary>
        /// Init
        /// </summary>
        public Scene(PictureBox canvas)
        {
            this.canvas = canvas;
            Camera = new Camera();
            ResetCamera();

            Light = new Vector3D(0, 150, -150);


        }

        public void ResetCamera()
        {
            Camera = new Camera(
                new Point3D(0, 0, -200),
                new Point3D(0, 0, 0), // или вариант поставить типа 0 0 200
                10,
                1000.0,
                canvas.ClientSize.Width,
                canvas.ClientSize.Height
            );
        }
        public void ResetLight()
        {
            Light = new Vector3D(0, 50, 0);
        }

        public void AddObject(RadioObject obj)
        {
            Objects.Add(obj);
        }

        public RadioObject GetObjectByName(string name)
        {
            foreach (RadioObject obj in Objects)
            {
                if (name.Equals(obj.ObjectName))
                {
                    return obj;
                }
            }
            return null;
        }

        public void DrawObjects()
        {
            FastBitmap bitmap = new FastBitmap(new Bitmap(Camera.Width, Camera.Height));
            Matrix cameraMatrix = Camera.GetCameraMatrix();

            // z-buffer (z-matrix). For each camera viewport pixel.
            // first, fill it with double.MaxValue with future z-buffer algoritm in mind.
            double[,] buffer = new double[Camera.Width, Camera.Height];
            for (int i = 0; i < Camera.Width; ++i)
                for (int j = 0; j < Camera.Height; ++j)
                    buffer[i, j] = double.MaxValue;

            if (IsPanning)
            { // defining light
                Matrix lightMatrix = ModificationMatrix.NewIdentityMatrix(4);
                var light = Light;
                {
                    Point3D lightPosition = new Point3D(light);
                }
            }

            if (SelectedForPanObject != null)
            {
                DrawObject(SelectedForPanObject, bitmap, buffer, cameraMatrix);
            }
            else
            {
                for (int i = 0; i < Objects.Count; ++i)
                {
                    SceneObject radioObject = Objects[i];
                    if (IsVisibleForCamera(radioObject.BasePoint, radioObject.MaxLength, cameraMatrix))
                    {
                        DrawObject(radioObject, bitmap, buffer, cameraMatrix);
                    }
                }
            }
            canvas.Image = bitmap.GetBitmap();
        }

        /// <summary>
        /// Draw the object on FastBitmap canvas using z-buffer and camera matrix
        /// </summary>
        private void DrawObject(SceneObject radioObject, FastBitmap bitmap, double[,] buffer, Matrix cameraMatrix)
        {
            // how basic object coordinates are modified (basePoint)
            Matrix objectMatrix = radioObject.NewModificationMatrix();
            foreach (PrimitiveWithName scenePrimitive in radioObject.Primitives)
            {
                // how current primitive coordinates are modified (basePoint)
                Matrix primitiveMatrix = scenePrimitive.GetModificationMatrix();
                foreach (Face face in scenePrimitive.Primitive.Faces)
                {
                    // add copy of local points (with z-buffer algoritm in mind)
                    List<Point3D> points = new List<Point3D>();
                    foreach (Point3D point in face.Points)
                    {
                        points.Add(point.Clone());
                    }

                    if (Mode == MODE.WIREFRAME)
                    { // then draw all the lines, the order is non-important
                        DrawObjectLines(bitmap, buffer, points, scenePrimitive.Primitive.Color, primitiveMatrix, scenePrimitive.Primitive.BasePoint, objectMatrix, radioObject.BasePoint, cameraMatrix);
                    }
                    else
                    {
                        // координаты объекта -> мировые
                        ApplyObjectMatrix(points, primitiveMatrix, scenePrimitive.Primitive.BasePoint, objectMatrix, radioObject.BasePoint);

                        ConvertWorldToCamera(points, cameraMatrix);
                        ConvertCameraToScreen(points);


                        // рисуем точки





                        // ok, let's do coloring
                        // calculating percent of color (brightness)
                        double brightness = CalculateLight(new Face(new Point3D[] { points[0], points[1], points[2] }));
                        Color color = Color.FromArgb(
                            (int)(scenePrimitive.Primitive.Color.R * brightness),
                            (int)(scenePrimitive.Primitive.Color.G * brightness),
                            (int)(scenePrimitive.Primitive.Color.B * brightness)
                        );

                        DrawTriangle(new Vector3D(points[0]), new Vector3D(points[1]), new Vector3D(points[2]), color, bitmap, buffer);
                    }
                }
            }
        }

        private double CalculateLight(Face face)
        {
            double result = 0;
            Vector3D normal = face.GetNormalVector();


            var light = Light;
            {
                result += Math.Abs(Vector3D.AngleCos(normal, light));
            }

            return Math.Min(result, 1);
        }

        private void DrawLines(FastBitmap bitmap, double[,] buffer, List<Point3D> points, Color color, Matrix objectMatrix, Point3D basePoint, Matrix cameraMatrix)
        {
            ConvertLocalToCamera(points, objectMatrix, basePoint, cameraMatrix);
            ConvertCameraToScreen(points);
            for (int pi = 0; pi < points.Count - 1; ++pi)
            {
                for (int pj = pi + 1; pj < points.Count; ++pj)
                {
                    DrawLine(bitmap, buffer, points[pi], points[pj], color);
                }
            }
        }

        private void DrawObjectLines(FastBitmap bitmap, double[,] buffer, List<Point3D> points, Color color, Matrix primitiveMatrix, Point3D primitiveBasePoint, Matrix objectMatrix, Point3D objectBasePoint, Matrix cameraMatrix)
        {
            // convert local object coordinates to camera coordinates, then to screen coords.
            ConvertLocalToCamera(points, primitiveMatrix, primitiveBasePoint, objectMatrix, objectBasePoint, cameraMatrix);
            ConvertCameraToScreen(points);
            for (int pi = 0; pi < points.Count - 1; ++pi)
            {
                for (int pj = pi + 1; pj < points.Count; ++pj)
                {
                    DrawLine(bitmap, buffer, points[pi], points[pj], color);
                }
            }
        }

        private void ConvertLocalToCamera(List<Point3D> points, Matrix objectMatrix, Point3D basePoint, Matrix cameraMatrix)
        {
            foreach (Point3D point in points)
            {
                Matrix result = objectMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0] + basePoint.X, result[1, 0] + basePoint.Y, result[2, 0] + basePoint.Z);
                result = cameraMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0], result[1, 0], result[2, 0]);
            }
        }

        private void ConvertLocalToCamera(List<Point3D> points, Matrix primitiveMatrix, Point3D primitiveBasePoint, Matrix objectMatrix, Point3D objectBasePoint, Matrix cameraMatrix)
        {
            foreach (Point3D point in points)
            {
                Matrix result = primitiveMatrix.Multiply(point.GetProjectiveCoordinates());
                var x = result[0, 0] + primitiveBasePoint.X;
                var y = result[1, 0] + primitiveBasePoint.Y;
                var z = result[2, 0] + primitiveBasePoint.Z;
                point.Set(x, y, z);

                result = objectMatrix.Multiply(point.GetProjectiveCoordinates());
                x = result[0, 0] + objectBasePoint.X;
                y = result[1, 0] + objectBasePoint.Y;
                z = result[2, 0] + objectBasePoint.Z;
                point.Set(x, y, z);

                result = cameraMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0], result[1, 0], result[2, 0]);
            }
        }

        private void ConvertWorldToCamera(List<Point3D> points, Matrix cameraMatrix)
        {
            foreach (Point3D point in points)
            {
                Matrix result = cameraMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0], result[1, 0], result[2, 0]);
            }
        }

        /// <summary>
        /// // convert object primitive coordinates to world coords.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="primitiveMatrix"></param>
        /// <param name="primitiveBasePoint"></param>
        /// <param name="objectMatrix"></param>
        /// <param name="objectBasePoint"></param>
        private void ApplyObjectMatrix(List<Point3D> points, Matrix primitiveMatrix, Point3D primitiveBasePoint, Matrix objectMatrix, Point3D objectBasePoint)
        {
            foreach (Point3D point in points)
            {
                // using the primitive projection...
                Matrix result = primitiveMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0] + primitiveBasePoint.X, result[1, 0] + primitiveBasePoint.Y, result[2, 0] + primitiveBasePoint.Z);

                // and then the object projection.
                result = objectMatrix.Multiply(point.GetProjectiveCoordinates());
                point.Set(result[0, 0] + objectBasePoint.X, result[1, 0] + objectBasePoint.Y, result[2, 0] + objectBasePoint.Z);
            }
        }

        private void ConvertCameraToScreen(List<Point3D> points)
        {
            foreach (Point3D point in points)
            {
                double x = point.X;
                double y = point.Y;
                double z = point.Z;

                if (Camera.IsCentralProjection && z != 0)
                {
                    x = Camera.FocusDistance * x / z;
                    y = Camera.FocusDistance * y / z;
                }

                x += Camera.HalfWidth;
                y += Camera.HalfHeight;

                point.Set((int)x, (int)y, z);
            }
        }

        /// <summary>
        /// Draws triangle on bitmap using specified point vectors and color (and z-buffer)
        /// </summary>
        private void DrawTriangle(Vector3D p0, Vector3D p1, Vector3D p2, Color color, FastBitmap bitmap, double[,] buffer)
        {
            if (p0.Y != p1.Y || p0.Y != p2.Y)
            {
                // упорядочение 
                if (p0.Y > p1.Y)
                {
                    MathHelpers.Swap(ref p0, ref p1);
                }
                if (p0.Y > p2.Y)
                {
                    MathHelpers.Swap(ref p0, ref p2);
                }
                if (p1.Y > p2.Y)
                {
                    MathHelpers.Swap(ref p1, ref p2);
                }

                // in the next section, I turn the triangle into points, then draw them (using z-buffer): split the triangle into 2 segments (vertically), and rasterize them separately. The algorithm is described here: https://compgraphics.info/2D/triangle_rasterization.php
                int totalHeight = (int)Math.Round(p2.Y - p0.Y);
                for (int i = 0; i < totalHeight; ++i)
                {
                    bool secondHalf = i > p1.Y - p0.Y || p1.Y == p0.Y;
                    int segmentHeight = (int)Math.Round(secondHalf ? p2.Y - p1.Y : p1.Y - p0.Y);
                    double alpha = (double)i / totalHeight;
                    double beta = (i - (secondHalf ? p1.Y - p0.Y : 0.0)) / segmentHeight;
                    Vector3D a = p0 + (p2 - p0) * alpha;
                    Vector3D b = (secondHalf ? p1 + (p2 - p1) * beta : p0 + (p1 - p0) * beta);

                    if (a.X > b.X)
                    { // I want first to be greater than second
                        MathHelpers.Swap(ref a, ref b);
                    }
                    for (int j = (int)Math.Round(a.X); j <= (int)Math.Round(b.X); ++j)
                    {
                        double scale = (a.X == b.X) ? 1 : (j - a.X) / (b.X - a.X);
                        Vector3D p = a + (b - a) * scale;
                        int x = (int)Math.Round(p.X);
                        int y = (int)Math.Round(p.Y);
                        if (IsWithinImage(x, y) && buffer[x, y] >= p.Z)
                        {
                            // z-buffer, we remember this new point
                            buffer[x, y] = p.Z;
                            bitmap.SetPixel(x, y, color);
                        }
                    }
                }
            }
        }

        private bool IsVisibleForCamera(Point3D point, double maxLength, Matrix cameraMatrix)
        {
            bool result = false;

            Point3D _point = point.Clone();
            // camera coords to world
            Matrix cameraToBase = cameraMatrix.Multiply(_point.GetProjectiveCoordinates());
            _point.Set(cameraToBase[0, 0], cameraToBase[1, 0], cameraToBase[2, 0]);

            // if the point is within Clipping Panes
            if (_point.Z + maxLength < Camera.FarClipZ && _point.Z - maxLength > Camera.NearClipZ)
            {
                if (Camera.IsCentralProjection)
                {
                    double testZ = Camera.HalfWidth * _point.Z / Camera.FocusDistance;

                    if (_point.X - maxLength < testZ && _point.X + maxLength > -testZ)
                    {
                        testZ = Camera.HalfHeight * _point.Z / Camera.FocusDistance;

                        if (_point.Y - maxLength < testZ && _point.Y + maxLength > -testZ)
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    if (Math.Abs(_point.X) + maxLength < Camera.HalfWidth && Math.Abs(_point.Y) + maxLength < Camera.HalfHeight)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Is within canvas
        /// </summary>
        private bool IsWithinImage(int x, int y)
        {
            return x > 0 && x < canvas.ClientSize.Width && y > 0 && y < canvas.ClientSize.Height;
        }

        /// <summary>
        /// Draw line on the bitmap. Using z-buffer and color.
        /// </summary>
        private void DrawLine(FastBitmap bitmap, double[,] buffer, Point3D start, Point3D end, Color color)
        {
            // dx and dy are coord diffs between two points
            double diffx = end.X - start.X;
            double diffy = end.Y - start.Y;

            if (Math.Abs(diffx) > Math.Abs(diffy))
            {
                // I want to work with point1 is 'greater' than point2
                if (diffx < 0)
                {
                    MathHelpers.Swap(ref start, ref end);
                }
                // here we are getting the segment coordinates as an array of Double values
                double[] yCoords = MathHelpers.GetYsByXsBetweenTwoPoints2D(start.X, start.Y, end.X, end.Y);
                double[] zCoords = MathHelpers.GetYsByXsBetweenTwoPoints2D(start.X, start.Z, end.X, end.Z);

                // and here we draw the points
                for (double x = start.X; x <= end.X; ++x)
                {
                    int y = (int)yCoords[(int)(x - start.X)];
                    double z = zCoords[(int)(x - start.X)];
                    if (IsWithinImage((int)x, y) && buffer[(int)x, y] >= z)
                    {
                        // in fact, there is smth like ray-tracing
                        // using z-buffer: if current point is closer to us than old one, we remember this...
                        buffer[(int)x, y] = z;
                        // and draw the point on the image. At last, the nearest point is drawing, other don't.
                        bitmap.SetPixel((int)x, y, color);
                    }
                }
            }
            else
            {
                // I want to work with point1 is 'greater' than point2
                if (diffy < 0)
                {
                    MathHelpers.Swap(ref start, ref end);
                }
                double[] xCoords = MathHelpers.GetYsByXsBetweenTwoPoints2D(start.Y, start.X, end.Y, end.X);
                double[] zCoords = MathHelpers.GetYsByXsBetweenTwoPoints2D(start.Y, start.Z, end.Y, end.Z);
                for (double y = start.Y; y <= end.Y; y++)
                {
                    int x = (int)xCoords[(int)(y - start.Y)];
                    double z = zCoords[(int)(y - start.Y)];
                    if (IsWithinImage(x, (int)y) && buffer[x, (int)y] >= z)
                    {
                        buffer[x, (int)y] = z;
                        bitmap.SetPixel(x, (int)y, color);
                    }
                }

            }
        }
    }
}