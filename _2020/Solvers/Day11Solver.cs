using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace _2020.Solvers
{
    public class Day11Solver : ISolver
    {
        private IDictionary<Point, Seat> _seats;
        private int _maxX;
        private int _maxY;
        
        public void Solve(string input)
        {
            this.ProcessSeats(input);
            
            var hasChangesV1 = true;
            var hasChangesV2 = true;

            do
            {
                // this.PrintSeats();
                
                foreach (var (_, seat) in this._seats)
                {
                    seat.StartNewIteration();
                }

                var nextHasChangesV1 = false;
                var nextHasChangesV2 = false;

                foreach (var seat in this._seats.Values)
                {
                    var (part1Update, part2Update) = seat.Update(hasChangesV1, hasChangesV2);

                    nextHasChangesV1 = nextHasChangesV1 || part1Update;
                    nextHasChangesV2 = nextHasChangesV2 || part2Update;
                }

                hasChangesV1 = nextHasChangesV1;
                hasChangesV2 = nextHasChangesV2;
            } while (hasChangesV1 || hasChangesV2);
            
            // this.PrintSeats();
            
            Console.WriteLine(this._seats.Values.Count(s => s.IsOccupied().v1));
            Console.WriteLine(this._seats.Values.Count(s => s.IsOccupied().v2));
        }

        private void PrintSeats()
        {
            for (var y = 0; y <= this._maxY; y++)
            {
                var line = "";
                
                for (var x = 0; x <= this._maxX; x++)
                {
                    if (this._seats.TryGetValue(new Point(x, y), out var seat))
                    {
                        line += seat.IsOccupied().v2 ? '#' : 'L';
                    }
                    else
                    {
                        line += '.';
                    }
                }
                
                Console.WriteLine(line);
            }
            
            Console.WriteLine();
            Console.WriteLine("--------");
            Console.WriteLine();
        }

        private void ProcessSeats(string input)
        {
            this._seats = new Dictionary<Point, Seat>();

            var x = 0;
            var y = 0;

            foreach (var c in input)
            {
                switch (c)
                {
                    case '\n':
                        this._maxX = x - 1;
                        x = 0;
                        y++;
                        break;
                    case 'L':
                        var seat = new Seat(x++, y);
                        this.DoAdjacentSeatStuff(seat);
                        this._seats.Add(seat.GetPos(), seat);
                        break;
                    case '.':
                        x++;
                        break;
                    default:
                        throw new ArgumentException($"Unexpected character: '{c}'");
                }
            }

            this._maxY = y;
        }

        private void DoAdjacentSeatStuff(Seat seat)
        {
            var seatPos = seat.GetPos();

            for (var dy = -1; dy <= 0; dy++)
            {
                for (var dx = -1; dx <= 1; dx++)
                {
                    if (dy == 0 && dx == 0)
                    {
                        // Look in all up directions & look left, as we will have already processed all these seats
                        break;
                    }

                    var curPos = new Point(seatPos.X + dx, seatPos.Y + dy);
                    
                    if (this._seats.TryGetValue(curPos, out var adjacentSeat))
                    {
                        seat.AddAdjacentSeat(adjacentSeat);
                        adjacentSeat.AddAdjacentSeat(seat);
                    }
                    else
                    {
                        // Try to find long adjacent seat
                        while (curPos.X >= 0 && curPos.Y >= 0)
                        {
                            if (this._seats.TryGetValue(curPos, out var longAdjacentSeat))
                            {
                                seat.AddLongAdjacentSeat(longAdjacentSeat);
                                longAdjacentSeat.AddLongAdjacentSeat(seat);
                                break;
                            }
                            
                            curPos.Offset(dx, dy);
                        }
                    }
                }
            }
        }

        private class Seat
        {
            private readonly Point _pos;
            private readonly HashSet<Seat> _adjacentSeats = new HashSet<Seat>();
            private bool _isOccupied = true;
            private bool _wasOccupied = true;

            private readonly HashSet<Seat> _longAdjacentSeats = new HashSet<Seat>();
            private bool _isOccupiedV2 = true;
            private bool _wasOccupiedV2 = true;

            public Seat(int x, int y)
            {
                _pos = new Point(x, y);
            }

            public Point GetPos()
            {
                return this._pos;
            }

            public (bool v1, bool v2) IsOccupied()
            {
                return (this._isOccupied, this._isOccupiedV2);
            }

            public void AddAdjacentSeat(Seat s)
            {
                this._adjacentSeats.Add(s);
            }

            public void AddLongAdjacentSeat(Seat s)
            {
                this._longAdjacentSeats.Add(s);
            }

            public void StartNewIteration()
            {
                this._wasOccupied = this._isOccupied;
                this._wasOccupiedV2 = this._isOccupiedV2;
            }

            public (bool part1Update, bool part2Update) Update(bool runPart1, bool runPart2)
            {
                var part1Update = false;
                
                if (runPart1)
                {
                    var numOccupiedAdjacent = this._adjacentSeats.Count(s => s._wasOccupied);

                    if (this._wasOccupied)
                    {
                        if (numOccupiedAdjacent >= 4)
                        {
                            // Was occupied last round, and has at least 4 adjacent occupied seats so is now unoccupied
                            this._isOccupied = false;
                        }
                    }
                    else
                    {
                        if (numOccupiedAdjacent == 0)
                        {
                            // Was empty last round, and all adjacent seats were also empty => becomes occupied
                            this._isOccupied = true;
                        }
                    }
                    
                    part1Update = this._wasOccupied != this._isOccupied;
                }

                var part2Update = false;

                if (runPart2)
                {
                    var numOccupiedLongAdjacent = this._adjacentSeats.Count(s => s._wasOccupiedV2) +
                                                  this._longAdjacentSeats.Count(s => s._wasOccupiedV2);
                    
                    if (this._wasOccupiedV2)
                    {
                        if (numOccupiedLongAdjacent >= 5)
                        {
                            // Was occupied last round, and has at least 5 "adjacent" occupied seats so is now unoccupied
                            this._isOccupiedV2 = false;
                        }
                    }
                    else
                    {
                        if (numOccupiedLongAdjacent == 0)
                        {
                            // Was empty last round, and all "adjacent" seats were also empty => becomes occupied
                            this._isOccupiedV2 = true;
                        }
                    }
                    
                    part2Update = this._wasOccupiedV2 != this._isOccupiedV2;
                }

                return (part1Update, part2Update);
            }
        }
    }
}