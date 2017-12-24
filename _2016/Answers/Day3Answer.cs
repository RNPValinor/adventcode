using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2016.Answers
{
    public class Day3Answer : IAnswer
    {
        private List<Triangle> Triangles;

        private void Init()
        {
            string line;
            var file = new StreamReader("Data/day3triangles.txt");
            this.Triangles = new List<Triangle>();

            while ((line = file.ReadLine()) != null)
            {
                var lineData = line.Split(' ');

                var tData = new List<int>();

                foreach (var side in lineData)
                {
                    var trimmed = side.Trim();

                    if (trimmed.Length > 0)
                    {
                        tData.Add(int.Parse(trimmed));
                    }
                }

                this.Triangles.Add(new Triangle(tData[0], tData[1], tData[2]));
            }
        }

        private void InitTwo()
        {
            string line;
            var file = new StreamReader("Data/day3triangles.txt");

            this.Triangles = new List<Triangle>();
            
            var triangleCache = new List<Triangle>();

            while ((line = file.ReadLine()) != null)
            {
                var lineData = line.Split(' ');

                var tData = new List<int>();

                foreach (var side in lineData)
                {
                    var trimmed = side.Trim();

                    if (trimmed.Length > 0)
                    {
                        tData.Add(int.Parse(trimmed));
                    }
                }

                if (triangleCache.Count() > 0)
                {
                    for (var i = 0; i < triangleCache.Count(); i++)
                    {
                        triangleCache[i].AddSide(tData[i]);
                    }

                    if (triangleCache[0].Complete())
                    {
                        this.Triangles.AddRange(triangleCache);
                        triangleCache.Clear();
                    }
                }
                else
                {
                    foreach (var side in tData)
                    {
                        var triangle = new Triangle();
                        triangle.AddSide(side);
                        triangleCache.Add(triangle);
                    }
                }
            }
        }

        public void PartOne()
        {
            this.Init();
            
            var numPossible = 0;

            foreach (var triangle in this.Triangles)
            {
                if (triangle.IsPossible())
                {
                    numPossible++;
                }
            }

            Console.WriteLine("Num possible triangles: " + numPossible);
        }

        public void PartTwo()
        {
            this.InitTwo();

            var numPossible = this.Triangles.Select(t => t.IsPossible()).Where(p => p).Count();

            Console.WriteLine("New num possible triangles: " + numPossible);
        }
    }

    class Triangle
    {
        private int X { get; set; }
        private int Y { get; set; }
        private int Z { get; set; }

        public Triangle()
        {
            
        }

        public Triangle(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public bool Complete()
        {
            return this.X != 0 && this.Y != 0 && this.Z != 0;
        }

        public void AddSide(int side)
        {
            if (this.X == 0)
            {
                this.X = side;
            }
            else if (this.Y == 0)
            {
                this.Y = side;
            }
            else
            {
                this.Z = side;
            }
        }

        public bool IsPossible()
        {
            var v1 = this.X + this.Y > this.Z;
            var v2 = this.Y + this.Z > this.X;
            var v3 = this.Z + this.X > this.Y;

            return v1 && v2 && v3;
        }

        public override string ToString()
        {
            return "(" + this.X + ", " + this.Y + ", " + this.Z + ")";
        }
    }
}