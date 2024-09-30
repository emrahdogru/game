using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain
{
    public struct Point
    {
        public Point()
        { }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X;
        public double Y;

        public double CalculateDistance(Point other)
        {
            return Math.Sqrt(Math.Pow(other.X - this.X, 2) + Math.Pow(other.Y - this.Y, 2));
        }

        /// <summary>
        /// <paramref name="a"/> noktasından <paramref name="b"/> noktasında toplam <paramref name="totalTime"/> zamanda giden bir birliğin, <paramref name="T"/> anındaki konumunu hesaplar.
        /// </summary>
        /// <param name="a">A noktası</param>
        /// <param name="b">B noktası</param>
        /// <param name="T">T anı</param>
        /// <param name="totalTime">Toplam yolculuk süresi</param>
        /// <returns></returns>
        public static Point CalculatePosition(Point a, Point b, TimeSpan T, TimeSpan totalTime)
        {
            // Cismin x ve y ekseninde toplam yer değiştirmesi
            double deltaX = b.X - a.X;
            double deltaY = b.Y - a.Y;

            // Zaman T anında x ve y koordinatları
            double xT = a.X + (deltaX * T.TotalMilliseconds / totalTime.TotalMilliseconds);
            double yT = a.Y + (deltaY * T.TotalMilliseconds / totalTime.TotalMilliseconds);

            return new Point(xT, yT);
        }

        /// <summary>
        /// <paramref name="a"/> noktasından <paramref name="b"/> noktasına giden birliğin açısını hesaplar
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double CalculateAngle(Point a, Point b)
        {
            // Açı (radyan cinsinden)
            double angleInRadians = Math.Atan2(b.Y - a.Y, b.X - a.X);

            // Açı (derece cinsinden)
            double angleInDegrees = angleInRadians * (180.0 / Math.PI);

            return angleInDegrees;
        }
    }
}
