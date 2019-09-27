using System.Drawing;
using System.Collections.Generic;

namespace Telescopes
{
    [System.Serializable]
    public class Primitive
    {
        public Color Color;

        /// <summary>
        /// basePoint in Decart / world coords. It can be treated as middle bottom point of any object.
        /// </summary>
        public Point3D BasePoint;

        public List<Face> Faces { get; protected set; } = new List<Face>();

        public Primitive(Point3D basePoint, Color color)
        {
            BasePoint = basePoint;
            Color = color;
        }
    }
}
