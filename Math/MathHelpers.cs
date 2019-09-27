using System;
using System.Collections.Generic;

namespace Telescopes
{
    public static class MathHelpers
    {
        /// <summary>
        /// Нахождение промежуточных значений. Для каждого X получаем соотв. Y.
        /// Функция написана для 2D, для 3D нужно будет вызвать для обоих 2D измерений.
        /// </summary>
        public static double[] GetYsByXsBetweenTwoPoints2D(double x1, double y1, double x2, double y2)
        {
            var diffY = y2 - y1;
            var diffX = x2 - x1;

            double[] values;
            if (x1 == x2)
            {
                values = new double[] { y1 };
            }
            else
            {
                values = new double[(int)Math.Abs(x2 - x1 + 1)];
                // шаг изменения y = высота/ширина
                double step = diffY/diffX;
                // начальное значение
                double value = y1;
                for (var i = x1; i <= x2; ++i)
                {
                    values[(int)(i - x1)] = value;
                    value += step;
                }
            }
            return values;
        }

        /// <summary>
        /// Получение промежуточных точек линии. Интерполирование, можно сказать.
        /// </summary>
        /// <param name="p1">start point</param>
        /// <param name="p2">end point</param>
        /// <returns></returns>
        public static Point3D[] GetLine3DPoints(Point3D start, Point3D end)
        {
            var points = new List<Point3D>();

            double diffx = end.X - start.X;
            double diffy = end.Y - start.Y;

            if (Math.Abs(diffx) > Math.Abs(diffy)) // abs - убирание знака
            {
                // упорядочиваем точки по X
                if (diffx < 0)
                {
                    MathHelpers.Swap(ref start, ref end);
                }

                // получаем массив координат
                // их длина = |diffx|+1 (потому что рисуем отрезок, с начальной И конечной точкой.)
                double[] yCoords = MathHelpers.GetYsByXsBetweenTwoPoints2D(start.X, start.Y, end.X, end.Y);
                double[] zCoords = MathHelpers.GetYsByXsBetweenTwoPoints2D(start.X, start.Z, end.X, end.Z);

                // идем по X, получаем Y и Z
                for (double x = start.X; x <= end.X; ++x)
                {
                    //(x - start.X) и так будет int.
                    int y = (int)yCoords[(int)(x - start.X)];
                    double z = zCoords[(int)(x - start.X)];

                    var point = new Point3D(x, y, z);
                    points.Add(point);
                }
            }
            else
            {
                // упорядочиваем точки по y
                if (diffy < 0)
                {
                    MathHelpers.Swap(ref start, ref end);
                }
                double[] xCoords = MathHelpers.GetYsByXsBetweenTwoPoints2D(start.Y, start.X, end.Y, end.X);
                double[] zCoords = MathHelpers.GetYsByXsBetweenTwoPoints2D(start.Y, start.Z, end.Y, end.Z);

                // идем по Y, получаем X и Z
                for (double y = start.Y; y <= end.Y; y++)
                {
                    int x = (int)xCoords[(int)(y - start.Y)];
                    double z = zCoords[(int)(y - start.Y)];

                    var point = new Point3D(x, y, z);
                    points.Add(point);
                }

            }


            return points.ToArray();
        }

        public static double FromDegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double FromRadiansToDegrees(double angle)
        {
            return angle * 180.0 / Math.PI;
        }

        public static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        public static Point3D GetMidPoint(this System.Collections.Generic.IList<TelescopeObject> objects)
        {
            var total = new Point3D(0,0,0);

            foreach(var obj in objects)
            {
                var point = obj.BasePoint;

                total.X += point.X;
                total.Y += point.Y;
                total.Z += point.Z;
            }

            total.X /= objects.Count;
            total.Y /= objects.Count;
            total.Z /= objects.Count;

            return total;
        }

        public static double GetMaxDistanceFrom(this System.Collections.Generic.IList<TelescopeObject> objects, Point3D target)
        {
            if (objects.Count is 0)
                return 0;

            var d = GetSegmentLength(objects[0].BasePoint, target);

            foreach(var obj in objects)
            {
                var d1 = GetSegmentLength(obj.BasePoint, target);
                if (d1 > d)
                {
                    d = d1;
                }
            }


            return d;
        }

        public static Point3D GetMidPoint(this Face face)
        {
            var total = new Point3D(0, 0, 0);

            foreach (var point in face.Points)
            {
                total.X += point.X;
                total.Y += point.Y;
                total.Z += point.Z;
            }


            total.X /= face.Points.Length;
            total.Y /= face.Points.Length;
            total.Z /= face.Points.Length;

            return total;
        }

        /// <summary>
        /// Get area of the triangular face using Geron formula
        /// </summary>
        /// <returns>double area</returns>
        public static double GetArea(this Face face)
        {
            var point1 = face.Points[0];
            var point2 = face.Points[1];
            var point3 = face.Points[2];

            double a, b, c;
            a = GetSegmentLength(point1, point2);
            b = GetSegmentLength(point2, point3);
            c = GetSegmentLength(point3, point1);

            var perimeter = a + b + c;
            double p = perimeter / 2;

            double S = 0;
            S = Math.Sqrt(p*(p-a) * (p - b) * (p - c));


            return S;
        }

        public static double GetSegmentLength(Point3D a, Point3D b)
        {
            double ab;
            ab = sqrt(sqr(b.X - a.X) + sqr(b.Y - a.Y) + sqr(b.Z - a.Z));

            return ab;
        }

        public static double sqrt(double x)
        {
            return Math.Sqrt(x);
        }

        public static double sqr(double x)
        {
            return Math.Pow(x,2);
        }
    }
}
