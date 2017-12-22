using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using _2016.Utils;

namespace _2016.Answers
{
    public class Day1Answer : IAnswer
    {
        private IList<string> instructions;
        private Point position;
        private Velocity velocity;

        private void Init() {
            var data = File.ReadAllText("Data/day1moves.txt");

            this.instructions = data.Split(", ");
            this.position = new Point(0, 0);
            this.velocity = new Velocity
            {
                X = 0,
                Y = -1
            };
        }

        private void Turn(char dir) {
            if (dir == 'R')
            {
                if (this.velocity.Y != 0)
                {
                    this.velocity.X = this.velocity.Y;
                    this.velocity.Y = 0;
                }
                else
                {
                    this.velocity.Y = -this.velocity.X;
                    this.velocity.X = 0;
                }
            }
            else if (dir == 'L')
            {
                if (this.velocity.Y != 0) {
                    this.velocity.X = -this.velocity.Y;
                    this.velocity.Y = 0;
                }
                else
                {
                    this.velocity.Y = this.velocity.X;
                    this.velocity.X = 0;
                }
            }
        }

        public void PartOne()
        {
            this.Init();

            foreach (var instruction in this.instructions)
            {
                this.Turn(instruction[0]);
                var distance = int.Parse(instruction.Substring(1));

                this.position.X += distance * this.velocity.X;
                this.position.Y += distance * this.velocity.Y;
            }

            Console.WriteLine("Final position is " + this.position.ToString());
            Console.WriteLine("Distance is " + (Math.Abs(this.position.X) + Math.Abs(this.position.Y)));
        }
        
        private bool HasBeenSeen(List<Point> points, Point newPoint)
        {
            foreach (var seenPoint in points) {
                if (seenPoint.Equals(newPoint))
                {
                    return true;
                }
            }

            return false;
        }

        public void PartTwo()
        {
            this.Init();

            var visited = new List<Point>();
            visited.Add(this.position);

            foreach (var instruction in this.instructions)
            {
                this.Turn(instruction[0]);

                var distance = int.Parse(instruction.Substring(1));

                while (distance > 0) {
                    this.position = new Point(this.position.X + this.velocity.X, this.position.Y + this.velocity.Y);

                    if (this.HasBeenSeen(visited, this.position)) {
                        Console.WriteLine("Duplicate position: " + this.position);
                        return;
                    }

                    visited.Add(this.position);
                    distance--;
                }
            }
        }
    }
}