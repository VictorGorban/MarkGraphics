using System;

namespace Game_Consoles
{
    /// <summary>
    /// Класс для реализации матрицы изменения
    /// </summary>
    public class ModificationMatrix
    {
        

        public static Matrix NewIdentityMatrix(int n)
        {
            double[,] result = new double[n, n];
            for (int i = 0; i < n; ++i) {
                for (int j = 0; j < n; ++j) {
                    if (i == j) {
                        result[i, j] = 1.0;
                    } else {
                        result[i, j] = 0.0;
                    }
                }
            }
            return new Matrix(result);
        }

        public static Matrix NewMoveMatrix(double dx, double dy, double dz)
        {
            var moveMatrix = NewIdentityMatrix(4);
        
        moveMatrix[0, 3] = dx;
            moveMatrix[1, 3] = dy;
            moveMatrix[2, 3] = dz;

            return moveMatrix;
        }

        public static Matrix NewScaleMatrix(double sx, double sy, double sz)
        {
            var scaleMatrix = NewIdentityMatrix(4);


            scaleMatrix[0, 0] = sx;
            scaleMatrix[1, 1] = sy;
            scaleMatrix[2, 2] = sz;

            return scaleMatrix;
        }

        public static Matrix NewScaleMatrix(double scale)
        {
            return NewScaleMatrix(scale, scale, scale);
        }

        public static Matrix NewRotateXMatrix(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            var rotateXMatrix = NewIdentityMatrix(4);


            rotateXMatrix[1, 1] = cos;
            rotateXMatrix[2, 1] = -sin;
            rotateXMatrix[1, 2] = sin;
            rotateXMatrix[2, 2] = cos;

            return rotateXMatrix;
        }

        public static Matrix NewRotateYMatrix(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            var rotateYMatrix = NewIdentityMatrix(4);

            rotateYMatrix[0, 0] = cos;
            rotateYMatrix[0, 2] = -sin;
            rotateYMatrix[2, 0] = sin;
            rotateYMatrix[2, 2] = cos;

            return rotateYMatrix;
        }

        public static Matrix NewRotateZMatrix(double angle)
        {
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            var rotateZMatrix = NewIdentityMatrix(4);
            rotateZMatrix[0, 0] = cos;
            rotateZMatrix[1, 0] = -sin;
            rotateZMatrix[0, 1] = sin;
            rotateZMatrix[1, 1] = cos;

            return rotateZMatrix;
        }

        /// <summary>
        /// Для камеры. u, v, n - это векторы. u = x, v - вертикальный (y), n - нормальный (z)
        /// </summary>
        public static Matrix NewUVNProjectionMatrix(double ux, double uy, double uz, double vx, double vy, double vz, double nx, double ny, double nz)
        {
            var uvnProjectionMatrix = NewIdentityMatrix(4);

            uvnProjectionMatrix[0, 0] = ux;
            uvnProjectionMatrix[1, 0] = vx;
            uvnProjectionMatrix[2, 0] = nx;

            uvnProjectionMatrix[0, 1] = uy;
            uvnProjectionMatrix[1, 1] = vy;
            uvnProjectionMatrix[2, 1] = ny;

            uvnProjectionMatrix[0, 2] = uz;
            uvnProjectionMatrix[1, 2] = vz;
            uvnProjectionMatrix[2, 2] = nz;

            return uvnProjectionMatrix;
        }
    }
}
