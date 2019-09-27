using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Telescopes
{
    public class Sphere : Primitive
    {
        public double Radius { get; private set; }
        public int SegmentsCount { get; private set; }

        public Sphere(Point3D basePoint, double radius, int segmentsCount, Color color) : base(basePoint, color)
        {
            Radius = radius;
            SegmentsCount = Math.Max(segmentsCount, 4);
            UpdatePoints();
        }

        // все тут нормально с кодом
        private Point3D[] GetCirclePoints(double r, double segmentsCount, double y)
        {
            double angle = 360.0 / SegmentsCount;
            double sin = Math.Sin(MathHelpers.FromDegreesToRadians(angle));
            double cos = Math.Cos(MathHelpers.FromDegreesToRadians(angle));

            Point3D[] points = new Point3D[SegmentsCount + 1];
            points[0] = new Point3D(-r, y, 0);

            for (int i = 1; i <= SegmentsCount; ++i)
            {
                double x = points[i - 1].X * cos - points[i - 1].Z * sin;
                double z = points[i - 1].X * sin + points[i - 1].Z * cos;
                points[i] = new Point3D(x, y, z);
            }

            return points;
        }

        /// <summary>
        ///       horda
        ///  |-----------
        ///  |         -
        ///  |        -
        ///k |      -
        ///  |    - R
        ///  |  -
        ///  |-
        /// </summary>
        /// <param name="R">radius</param>
        /// <param name="k">height</param>
        /// <returns>horda</returns>

        private double GetHordaRadiusByHeight(double R, double k)
        {
            var b2 = MathHelpers.sqr(R) - MathHelpers.sqr(k);
            var b = MathHelpers.sqrt(b2);
            return b;
        }

        // отрисовка по методу "нарисуй кучу колец, потом соедини точки"
        private void UpdatePoints()
        {
            // Я могу получать точки, как в основании цилиндра, затем идти вверх/вниз и добавлять эти круги. Потом пройтись по циклу и добавить faces между ними.
            // разбил радиус по кол-ву сементов, чтобы был понятен радиус колец

            int levelsCount = SegmentsCount / 2; // от среднего уровня, где R кольца = max R, до нижнего уровня, где R кольца = 0;. Где R кольца = 0, мы не считаем, потому что будем рисовать нижний уровень как треугольники.
            double maxR = Radius;
            
            double dh = maxR / levelsCount; // изменение высоты (т.е. Y) на каждом кольце.
            double[] tempRs = new double[levelsCount * 2]; // from 0 to R, (from R to 0 but R)


            // заполнение 1 половины
            for (var i = 0; i < tempRs.Length / 2; i++) // до radiuses.Length не доходит
            {
                // здесь k будет максимальная
                // k is удаленность хорды от радиуса
                double k = ((double)tempRs.Length / 2 - i);
                k *= dh;
                var r = GetHordaRadiusByHeight(maxR, k);

                tempRs[i] = r;
            }

            // заполнение 2 половины
            for (var i = tempRs.Length / 2; i < tempRs.Length; i++)
            {
                double k = i - tempRs.Length / 2;
                k *= dh;
                var r = GetHordaRadiusByHeight(maxR, k);

                tempRs[i] = r;
            }

            var radiuses = new List<double>(tempRs);
            radiuses.Add(0); // last 0. Без этого не катит(

            var pointsByLevels = new List<Point3D[]>(radiuses.Count);

            for (var i = 0; i < radiuses.Count; i++)
            {
                var r = radiuses[i];

                //points on the level circle
                var ps = GetCirclePoints(r, SegmentsCount, i * dh);
                pointsByLevels.Add(ps);
            }

            // все, заполнили PointsByLevels, теперь триангулируем.
            Faces.Clear();

            // вроде все ок, теперь надо нарисовать на бумажке эти level circles с соединить точки

            for (var j = 0; j < pointsByLevels.Count - 1; j++) // 
            {
                var array1 = pointsByLevels[j];
                var array2 = pointsByLevels[j + 1];
                for (var i = 0; i < array1.Length - 1; i++)
                {
                    Point3D p11, p12, p21, p22;
                    p11 = array1[i];
                    p12 = array1[i + 1];
                    p21 = array2[i];
                    p22 = array2[i + 1];


                    Face f1, f2;
                    f1 = new Face(p11, p12, p21);
                    f2 = new Face(p21, p12, p22);

                    Faces.Add(f1);
                    Faces.Add(f2);
                }

            }
        }

        public void ModifyRadius(double radius)
        {
            Radius = radius;
            UpdatePoints();
        }

        public void ModifyBasePoint(Point3D basePoint)
        {
            BasePoint = basePoint;
            UpdatePoints();
        }
    }
}
