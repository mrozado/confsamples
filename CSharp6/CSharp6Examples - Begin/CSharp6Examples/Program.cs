﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp6Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var points = new List<Point> { new Point(3, 4), new Point(-1, 0), new Point(4, -5), new Point(3, 6) };

            Console.WriteLine(Point.ToString(points));

            var json = new JArray(from p in points select p.ToJson()).ToString();

            Console.WriteLine(json);

            var pointsAgain =
                from j in JArray.Parse(json)
                select Point.FromJson(j as JObject);

            Console.WriteLine(Point.ToString(pointsAgain));

            Console.ReadKey();
        }
    }

    class Point
    {
        private readonly int _x;
        private readonly int _y;

        public int X { get { return _x; } }
        public int Y { get { return _y; } }

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }

        public double Dist
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.X - p2.X, p1.Y - p2.Y);
        }

        public static Point FromJson(JObject json)
        {
            if (json == null ||
                json["x"] == null ||
                json["x"].Type != JTokenType.Integer ||
                json["y"] == null ||
                json["y"].Type != JTokenType.Integer)
            {
                throw new ArgumentException("The parameter is not shaped like a Point", "json");
            }

            return new Point((int)json["x"], (int)json["y"]);
        }

        public static string ToString(IEnumerable<Point> points)
        {
            var builder = new StringBuilder();

            foreach(var point in points)
            {
                builder.AppendLine(point.ToString());
            }

            return builder.ToString();
        }

        public object ToJson()
        {
            var result = new JObject();

            result["x"] = X;
            result["y"] = Y;

            return result;
        }

        private static async Task<JArray> GetJArrayAsync(string path)
        {
            var repository = await Repository.OpenAsync(path);

            try
            {
                var contents = await repository.ReadAsync();
                return JArray.Parse(contents);
            }
            catch (RepositoryException e)
            {

            }
            finally
            {
            }

            return new JArray();
        }

        public string ToJsonString()
        {
            var s = @"{
            ""x"": " + X + @",
            ""y"": " + Y + @"
            }";

            return s;
        }
    }

    class Repository
    {
        internal static Task<Repository> OpenAsync(string path)
        {
            throw new NotImplementedException();
        }

        internal Task<String> ReadAsync()
        {
            throw new NotImplementedException();
        }

        internal Task LogAsync(RepositoryException e)
        {
            throw new NotImplementedException();
        }

        internal Task Close()
        {
            throw new NotImplementedException();
        }
    }

    class RepositoryException : Exception
    {

    }


}
