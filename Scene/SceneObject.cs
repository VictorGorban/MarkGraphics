using System;
using System.Collections.Generic;

namespace Telescopes
{
    /// <summary>
    /// Класс, который содержит методы и свойства, которые в принципе можно вынести в отдельный класс из RadioObject.
    /// </summary>
    [Serializable]
    public class SceneObject
    {
        public string ObjectName { get; private set; }
        public Point3D BasePoint { get; set; }

        public List<PrimitiveWithName> Primitives { get; private set; } = new List<PrimitiveWithName>();

        public SceneObject() { }

        public SceneObject(string name)
        {
            ObjectName = name;
        }

        public void AddScenePrimitive(PrimitiveWithName scenePrimitive)
        {
            Primitives.Add(scenePrimitive);
        }

        public void SetObjectNameUnsafe(string name)
        {
            ObjectName = name;
        }

        public double ScaleX { get; private set; } = 1.0;
        public double ScaleY { get; private set; } = 1.0;
        public double ScaleZ { get; private set; } = 1.0;
        public int AngleX { get; set; } = 0;
        public int AngleY { get; set; } = 0;
        public int AngleZ { get; set; } = 0;
        private double maxLength = 0;
        public double MaxLength
        {
            get
            {
                if (maxLength == 0)
                {
                    double result = 0;
                    foreach (PrimitiveWithName primitive in Primitives)
                    {
                        result = Math.Max(primitive.MaxLength, result);
                    }
                    maxLength = result * Math.Max(Math.Max(ScaleX, ScaleY), ScaleZ);
                }
                return maxLength;
            }
        }

        public void SetScale(double scaleX, double scaleY, double scaleZ)
        {
            ScaleX = scaleX;
            ScaleY = scaleY;
            ScaleZ = scaleZ;
            maxLength = 0;
        }

        /// <summary>
        /// Scale, rotate
        /// </summary>
        public Matrix NewModificationMatrix()
        {
            Matrix matrix = ModificationMatrix.NewIdentityMatrix(4);
            matrix = matrix.Multiply(ModificationMatrix.NewScaleMatrix(ScaleX, ScaleY, ScaleZ));
            matrix = matrix.Multiply(ModificationMatrix.NewRotateXMatrix(MathHelpers.FromDegreesToRadians(AngleX)));
            matrix = matrix.Multiply(ModificationMatrix.NewRotateYMatrix(MathHelpers.FromDegreesToRadians(AngleY)));
            matrix = matrix.Multiply(ModificationMatrix.NewRotateZMatrix(MathHelpers.FromDegreesToRadians(AngleZ)));
            return matrix;
        }

    }
}
