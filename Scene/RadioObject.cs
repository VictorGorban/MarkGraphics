﻿using System;
using System.Drawing;

namespace Game_Consoles
{
    [Serializable]
    public class RadioObject : SceneObject
    {
        public double Basis1CylinderRadius { get; set; } = 5;
        public double PrimaryLegsLength { get; set; } = 40;
        public double Basis2CylinderRadius { get; set; } = 8;
        public double Basis3CylinderRadius { get; set; } = 11;
        public double LenseRadius { get; set; } = 10;
        public double SecondaryLegsLength { get; set; } = 10;
        public double PrimaryLegsCount { get; set; } = 3;
        public double SecondaryLegsCount { get; set; } = 2;
        public int HandsCount { get; set; } = 2;
        public int HandsRadius { get; set; } = 1;

        public RadioObject() { }

        public RadioObject(string name) : base(name)
        {
            UpdateObject();
        }

        public void UpdateObject()
        {
            Primitives.Clear();

            var cdn = 5;
            //var centerLeg = new PrimitiveWithName(
            //    new Cylinder(new Point3D(0,0,0), cdn, PrimaryLegsLength, 10, Color.LightGray),
            //    "centerLeg"
            //    );
            //Primitives.Add(centerLeg);


            {
                var pcount = (int)PrimaryLegsCount;

                var r = PrimaryLegsLength / 3; // радиус зависит от длины -> одинаковый угол

                double angle = 360.0 / pcount;
                double sin = Math.Sin(MathHelpers.FromDegreesToRadians(angle));
                double cos = Math.Cos(MathHelpers.FromDegreesToRadians(angle));

                var points = new Point3D[pcount];
                points[0] = new Point3D(r, 0, r);

                for (int i = 1; i <= points.Length - 1; i++)
                {
                    double x = points[i - 1].X * cos - points[i - 1].Z * sin;
                    double z = points[i - 1].X * sin + points[i - 1].Z * cos;
                    points[i] = new Point3D(x, 0, z);
                }

                var legs = new PrimitiveWithName[points.Length];
                for (int i = 0; i < points.Length; i++)
                {
                    var leg = new PrimitiveWithName(
                    new Cylinder(points[i], cdn, PrimaryLegsLength, 10, Color.LightGray),
                    "leg" + i
                    );
                    Primitives.Add(leg);
                    legs[i] = leg;
                }

                if (legs.Length is 3)
                {
                    legs[0].AngleX = 23;
                    legs[0].AngleZ = -22;

                    legs[1].AngleX = 10;
                    legs[1].AngleZ = 28;

                    legs[2].AngleX = -27;
                    legs[2].AngleZ = -7;
                }
                if (legs.Length is 4)
                {
                    legs[0].AngleX = 20;
                    legs[0].AngleZ = -20;

                    legs[1].AngleX = 20;
                    legs[1].AngleZ = 20;

                    legs[2].AngleX = -20;
                    legs[2].AngleZ = 20;

                    legs[3].AngleX = -20;
                    legs[3].AngleZ = -20;
                }
            }



            //var basis = new PrimitiveWithName(
            //    new Box(new Point3D(0, 0, 0), 150, 50, 75, Color.SaddleBrown),
            //    "Basis"
            //    ); // раз все равно примитив по умолчанию с именем, почему бы не задать ему имя.
            //Primitives.Add(basis);

            //var sphere1 = new PrimitiveWithName(
            //    new Sphere(new Point3D(-20, 25, 37.5), 10, 16, Color.Brown),
            //    "Sphere1"
            //    );
            //Primitives.Add(sphere1);

            //if (HandsCount == 2)
            //{
            //    var sphere2 = new PrimitiveWithName(
            //    new Sphere(new Point3D(20, 25, 37.5), 10, 16, Color.Brown),
            //    "Sphere2"
            //    );
            //    Primitives.Add(sphere2);
            //}

            //var leftCylinderStand = new PrimitiveWithName(
            //    new Cylinder(new Point3D(-50, 50, 10), Basis3CylinderRadius, 5, 10, Color.LightGray),
            //    "leftCylinderStand"
            //    );
            //Primitives.Add(leftCylinderStand);

            //var rightCylinderStand = new PrimitiveWithName(
            //    new Cylinder(new Point3D(+50, 50, 10), Basis1CylinderRadius, 5, 10, Color.LightGray),
            //    "rightCylinderStand"
            //    );
            //Primitives.Add(rightCylinderStand);

            //var leftVolumeControllerWidestPart = new PrimitiveWithName(
            //    new Cylinder(new Point3D(-50, 55, 10), LenseRadius, 3, 10, Color.FromArgb(20, 20, 20)),
            //    "LeftVolumeControllerWidestPart"
            //    );
            //Primitives.Add(leftVolumeControllerWidestPart);

            //var rightVolumeControllerWidestPart = new PrimitiveWithName(
            //    new Cylinder(new Point3D(+50, 55, 10), PrimaryLegsLength, 3, 10, Color.FromArgb(20, 20, 20)),
            //    "rightVolumeControllerWidestPart"
            //    );
            //Primitives.Add(rightVolumeControllerWidestPart);

            //var leftVolumeControllerStripe = new PrimitiveWithName(
            //    new Cylinder(new Point3D(-50, 58, 10), SecondaryLegsLength, 1, 10, Color.FromArgb(240, 240, 240)),
            //    "leftVolumeControllerStripe"
            //    );
            //Primitives.Add(leftVolumeControllerStripe);

            //var rightVolumeControllerStripe = new PrimitiveWithName(
            //    new Cylinder(new Point3D(+50, 58, 10), Basis2CylinderRadius, 1, 10, Color.FromArgb(240, 240, 240)),
            //    "rightVolumeControllerStripe"
            //    );
            //Primitives.Add(rightVolumeControllerStripe);

            ////cassete decorations

            //var decor1 = new PrimitiveWithName(
            //    new Box(new Point3D(0, 50, 15), 70, 1, 20, Color.FromArgb(86, 86, 106)),
            //    "decor1"
            //    ); // раз все равно примитив по умолчанию с именем, почему бы не задать ему имя.
            //Primitives.Add(decor1);

            //var decor2 = new PrimitiveWithName(
            //    new Box(new Point3D(0, 51, 13.5), 53, 1, 11, Color.FromArgb(245, 222, 179)),
            //    "decor2"
            //    ); // раз все равно примитив по умолчанию с именем, почему бы не задать ему имя.
            //Primitives.Add(decor2);

            //var decorButton1 = new PrimitiveWithName(
            //    new Box(new Point3D(-30, 51, 13.5), 2.5, 1, 5, Color.DarkRed),
            //    "decorButton1"
            //    ); // раз все равно примитив по умолчанию с именем, почему бы не задать ему имя.
            //Primitives.Add(decorButton1);

            //var decorButton2 = new PrimitiveWithName(
            //    new Box(new Point3D(+30, 51, 15), 3, 1, 3, Color.Coral),
            //    "decorButton2"
            //    ); // раз все равно примитив по умолчанию с именем, почему бы не задать ему имя.
            //Primitives.Add(decorButton2);

            //var decorButton3 = new PrimitiveWithName(
            //    new Box(new Point3D(+30, 51, 10), 3, 1, 3, Color.Coral),
            //    "decorButton3"
            //    ); // раз все равно примитив по умолчанию с именем, почему бы не задать ему имя.
            //Primitives.Add(decorButton3);

            //var buttonPlay = new PrimitiveWithName(
            //    new Box(new Point3D(-62, 50, -30), SecondaryLegsCount, 1, PrimaryLegsCount, Color.FromArgb(86, 86, 106)),
            //    "buttonPlay"
            //    );
            //Primitives.Add(buttonPlay);

            //var buttonPlayStripe = new PrimitiveWithName(
            //    new BoxTriangle(new Point3D(-62, 51, -30), SecondaryLegsCount - 2, 1, PrimaryLegsCount - 2, Color.Green),
            //    "buttonPlayStripe"
            //    );
            //Primitives.Add(buttonPlayStripe);


            //var buttonStop = new PrimitiveWithName(
            //    new Box(new Point3D(-50, 50, -30), SecondaryLegsCount, 1, PrimaryLegsCount, Color.FromArgb(86, 86, 106)),
            //    "buttonStop"
            //    );
            //Primitives.Add(buttonStop);

            //var buttonStopStripe = new PrimitiveWithName(
            //    new Box(new Point3D(-50, 51, -30), SecondaryLegsCount - 2, 1, PrimaryLegsCount - 2, Color.Red),
            //    "buttonStopStripe"
            //    );
            //Primitives.Add(buttonStopStripe);


            //if (HandsRadius == 2)
            //{
            //    var buttonPlayBack = new PrimitiveWithName(
            //    new Box(new Point3D(50, 50, -30), SecondaryLegsCount, 1.5, PrimaryLegsCount, Color.FromArgb(86, 86, 106)),
            //    "buttonPlayBack"
            //    );
            //    Primitives.Add(buttonPlayBack);

            //    var buttonPlayBackStripe = new PrimitiveWithName(
            //    new BoxTriangleBack(new Point3D(50, 51, -30), SecondaryLegsCount - 2, 1, PrimaryLegsCount - 2, Color.FromArgb(240, 240, 240)),
            //    "buttonPlayBackStripe"
            //    );
            //    Primitives.Add(buttonPlayBackStripe);
            //}

            //var buttonPlayForward = new PrimitiveWithName(
            //    new Box(new Point3D(62, 50, -30), SecondaryLegsCount, 1, PrimaryLegsCount, Color.FromArgb(86, 86, 106)),
            //    "buttonPlayForward"
            //    );
            //Primitives.Add(buttonPlayForward);

            //var buttonPlayForwardStripe = new PrimitiveWithName(
            //    new BoxTriangle(new Point3D(62, 51, -30), SecondaryLegsCount - 2, 1, PrimaryLegsCount - 2, Color.FromArgb(240, 240, 240)),
            //    "buttonPlayForwardStripe"
            //    );
            //Primitives.Add(buttonPlayForwardStripe);

        }
    }
}
