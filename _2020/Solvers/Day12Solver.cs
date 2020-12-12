using System;
using System.Drawing;

namespace _2020.Solvers
{
    public class Day12Solver : ISolver
    {
        public void Solve(string input)
        {
            var instructions = input.Split(Environment.NewLine);
            
            var curPos = new Point(0, 0);
            var curFacing = Direction.E;

            var waypoint = new Point(10, 1);
            var realShipPos = new Point(0, 0);

            foreach (var instLine in instructions)
            {
                var inst = instLine[0];
                var amount = int.Parse(instLine.Substring(1));
                var rot = amount / 90;

                switch (inst)
                {
                    case 'N':
                        curPos.Offset(0, amount);
                        waypoint.Offset(0, amount);
                        break;
                    case 'S':
                        curPos.Offset(0, -amount);
                        waypoint.Offset(0, -amount);
                        break;
                    case 'E':
                        curPos.Offset(amount, 0);
                        waypoint.Offset(amount, 0);
                        break;
                    case 'W':
                        curPos.Offset(-amount, 0);
                        waypoint.Offset(-amount, 0);
                        break;
                    case 'L':
                        var nextFacing = ((int) curFacing - rot) % 4;
                        curFacing = (Direction) (nextFacing < 0 ? nextFacing + 4 : nextFacing);

                        while (rot > 0)
                        {
                            waypoint = new Point(-waypoint.Y, waypoint.X);
                            rot--;
                        }
                        break;
                    case 'R':
                        curFacing = (Direction) (((int) curFacing + rot) % 4);

                        while (rot > 0)
                        {
                            waypoint = new Point(waypoint.Y, -waypoint.X);
                            rot--;
                        }
                        break;
                    case 'F':
                        curPos.Offset(curFacing switch
                            {
                                Direction.E => amount,
                                Direction.W => -amount,
                                _ => 0
                            },
                            curFacing switch
                            {
                                Direction.N => amount,
                                Direction.S => -amount,
                                _ => 0
                            });

                        realShipPos.Offset(amount * waypoint.X, amount * waypoint.Y);
                        break;
                }
            }

            Console.WriteLine(Math.Abs(curPos.X) + Math.Abs(curPos.Y));
            Console.WriteLine(Math.Abs(realShipPos.X) + Math.Abs(realShipPos.Y));
        }

        private enum Direction
        {
            N, E, S, W
        }
    }
}