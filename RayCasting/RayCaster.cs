using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace RayCasting
{
    public class RayCaster
    {
        Pen Color;
        double AngleStep;
        double RayCastDistance;
        public RayCaster(Pen color, double angleStep, double rayCastDistance)
        {
            Color = color;
            AngleStep = angleStep;
            RayCastDistance = rayCastDistance;
        }

        public void Update(Graphics gfx, Point cursor, List<Line> barriers)
        {
            for(double angle = 0;angle < 2 *Math.PI;angle += AngleStep)
            {
                Point endpoint = new Point(cursor.X + (int)(Math.Cos(angle) * RayCastDistance), cursor.Y + (int)(Math.Sin(angle) * RayCastDistance));
                foreach(Line line in barriers)
                {
                    double denominator = (cursor.X - endpoint.X) * (line.Endpoints.Point1.Y - line.Endpoints.Point2.Y) - (cursor.Y - endpoint.Y) * (line.Endpoints.Point1.X - line.Endpoints.Point2.X);
                    if (denominator == 0) continue;
                    double t = ((cursor.X-line.Endpoints.Point1.X)*(line.Endpoints.Point1.Y - line.Endpoints.Point2.Y) - (cursor.Y - line.Endpoints.Point1.Y) * (line.Endpoints.Point1.X - line.Endpoints.Point2.X)) /denominator;
                    double u = -1 * ((cursor.X - endpoint.X) * (cursor.Y - line.Endpoints.Point1.Y) - (cursor.Y - endpoint.Y) * (cursor.X -line.Endpoints.Point1.X))/denominator;
                    if (!(0 <= t && t <= 1 && 0 <= u && u <= 1)) continue;
                    Point intersection = new Point((int)(cursor.X + t*(endpoint.X-cursor.X)), (int)(cursor.Y + t * (endpoint.Y - cursor.Y)));
                    endpoint = Line.GetDistance(cursor,intersection) < Line.GetDistance(cursor,endpoint) ? intersection : endpoint;
                }
                gfx.DrawLine(Color, cursor,endpoint);
            }
        }

        public static List<Line> GenerateBarriers(Graphics gfx, int numberOfBarriers, double maxLength, Bitmap canvas)
        {
            Random random = new Random();
            List<Line> retLines = new List<Line>();
            for (int i = 0;i<numberOfBarriers;i++)
            {
                Point firstPoint = new Point((int)(random.Next()%(canvas.Width-maxLength)), (int)(random.Next()%(canvas.Height-maxLength)));
                Point secondPoint = new Point(firstPoint.X + (int)(random.Next()%maxLength), firstPoint.Y + (int)(random.Next()%maxLength));
                retLines.Add(new Line(firstPoint, secondPoint));
            }
            return retLines;
        }

        public static void DrawBarriers(Graphics gfx, List<Line> lines, Pen color)
        {
            for (int i = 0; i < lines.Count; i++)
                gfx.DrawLine(color, lines[i].Endpoints.Point1, lines[i].Endpoints.Point2);
        }
    }

    public class Line
    {
        public (Point Point1, Point Point2) Endpoints;
        public Line(int x1,int y1, int x2,int y2)
        {
            Endpoints = (new Point(x1, y1), new Point(x2, y2));
        }
        public Line(Point firstPoint, Point secondPoint)
        {
            Endpoints = (firstPoint, secondPoint);
        }

        public static double GetDistance(Point point1,Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X,2) + Math.Pow(point1.Y - point2.Y,2));
        }
    }
}
