using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace _2016.Answers
{
    public class Day2Answer : IAnswer
    {
        private IList<string> moves;
        private Point pos;

        private void Init()
        {
            string line;
            var file = new StreamReader("Data/day2moves.txt");
            this.moves = new List<string>();

            while ((line = file.ReadLine()) != null)
            {
                moves.Add(line);
            }

            this.pos = new Point(1, 1);
        }

        private void Move(char dir)
        {
            switch (dir)
            {
                case 'U':
                    if (this.pos.Y > 0)
                        this.pos.Y--;
                    break;
                case 'D':
                    if (this.pos.Y < 2)
                        this.pos.Y++;
                    break;
                case 'L':
                    if (this.pos.X > 0)
                        this.pos.X--;
                    break;
                case 'R':
                    if (this.pos.X < 2)
                        this.pos.X++;
                    break;
            }
        }

        private void NewMove(char dir)
        {
            switch (dir)
            {
                case 'U':
                    if (this.pos.X == 1 || this.pos.X == 3)
                    {
                        if (this.pos.Y > 1)
                        {
                            this.pos.Y--;
                        }
                    }
                    else if (this.pos.X == 2)
                    {
                        if (this.pos.Y > 0)
                        {
                            this.pos.Y--;
                        }
                    }
                    break;
                case 'D':
                    if (this.pos.X == 1 || this.pos.X == 3)
                    {
                        if (this.pos.Y < 3)
                        {
                            this.pos.Y++;
                        }
                    }
                    else if (this.pos.X == 2)
                    {
                        if (this.pos.Y < 4)
                        {
                            this.pos.Y++;
                        }
                    }
                    break;
                case 'L':
                    if (this.pos.Y == 1 || this.pos.Y == 3)
                    {
                        if (this.pos.X > 1)
                        {
                            this.pos.X--;
                        }
                    }
                    else if (this.pos.Y == 2)
                    {
                        if (this.pos.X > 0)
                        {
                            this.pos.X--;
                        }
                    }
                    break;
                case 'R':
                    if (this.pos.Y == 1 || this.pos.Y == 3)
                    {
                        if (this.pos.X < 3)
                        {
                            this.pos.X++;
                        }
                    }
                    else if (this.pos.Y == 2)
                    {
                        if (this.pos.X < 4)
                        {
                            this.pos.X++;
                        }
                    }
                    break;
            }
        }

        private char ConvertPos()
        {
            var number = this.pos.Y * 3 + 1 + this.pos.X;

            return number.ToString()[0];
        }

        private char NewConvertPos()
        {
            switch (this.pos.Y)
            {
                case 0:
                    return '1';
                case 4:
                    return 'D';
                case 1:
                    return (this.pos.X + 1).ToString()[0];
                case 2:
                    return (this.pos.X + 5).ToString()[0];
                case 3:
                    switch (this.pos.X)
                    {
                        case 1:
                            return 'A';
                        case 2:
                            return 'B';
                        case 3:
                            return 'C';
                    }
                    break;
            }

            throw new ArgumentOutOfRangeException("Invalid co-ordinates: " + this.pos.ToString());
        }

        void IAnswer.PartOne()
        {
            this.Init();

            var num = "";

            foreach (var move in moves)
            {
                foreach (var dir in move)
                {
                    this.Move(dir);
                }

                num = num + this.ConvertPos();
            }

            Console.WriteLine("Code is " + num);
        }

        void IAnswer.PartTwo()
        {
            this.Init();

            this.pos = new Point(0, 2);
            var num = "";

            foreach (var move in moves)
            {
                foreach (var dir in move)
                {
                    this.NewMove(dir);
                }

                num = num + this.NewConvertPos();
            }

            Console.WriteLine("New code is " + num);
        }
    }
}