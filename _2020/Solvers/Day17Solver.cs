using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using _2020.Utils;
using VectorAndPoint.ValTypes;

namespace _2020.Solvers
{
    public class Day17Solver : ISolver
    {
        private HashSet<Point3D> _activeCubes = new();
        private HashSet<Point4D> _activeHyperCubes = new();
        
        public void Solve(string input)
        {
            var x = 0;
            var y = 0;

            foreach (var c in input)
            {
                switch (c)
                {
                    case '.':
                        x++;
                        break;
                    case '#':
                        this._activeCubes.Add(new Point3D(x, y, 0));
                        this._activeHyperCubes.Add(new Point4D(x, y, 0, 0));
                        x++;
                        break;
                    case '\n':
                        y++;
                        x = 0;
                        break;
                }
            }

            for (var i = 1; i <= 6; i++)
            {
                this.RunCycle();
            }

            Console.WriteLine(this._activeCubes.Count);
            Console.WriteLine(this._activeHyperCubes.Count);
        }

        private void RunCycle()
        {
            var neighbouringCubes = new ConcurrentDictionary<Point3D, int>();

            this._activeCubes.AsParallel().ForAll(c => ProcessActiveCube(c, neighbouringCubes));

            var nextActiveCubes = new HashSet<Point3D>();

            foreach (var (cube, numAdjacentActive) in neighbouringCubes)
            {
                switch (numAdjacentActive)
                {
                    case 2:
                        if (this._activeCubes.Contains(cube))
                        {
                            nextActiveCubes.Add(cube);
                        }
                        break;
                    case 3:
                        nextActiveCubes.Add(cube);
                        break;
                }
            }

            this._activeCubes = nextActiveCubes;
            
            var neighbouringHyperCubes = new ConcurrentDictionary<Point4D, int>();
            
            this._activeHyperCubes.AsParallel().ForAll(c => ProcessActiveHyperCube(c, neighbouringHyperCubes));

            var nextActiveHyperCubes = new HashSet<Point4D>();
            
            foreach (var (cube, numAdjacentActive) in neighbouringHyperCubes)
            {
                switch (numAdjacentActive)
                {
                    case 2:
                        if (this._activeHyperCubes.Contains(cube))
                        {
                            nextActiveHyperCubes.Add(cube);
                        }
                        break;
                    case 3:
                        nextActiveHyperCubes.Add(cube);
                        break;
                }
            }

            this._activeHyperCubes = nextActiveHyperCubes;
        }

        private static void ProcessActiveCube(Point3D cube, ConcurrentDictionary<Point3D, int> neighbouringCubes)
        {
            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    for (var dz = -1; dz <= 1; dz++)
                    {
                        if (dx == 0 && dy == 0 && dz == 0)
                        {
                            continue;
                        }

                        var neighbour = new Point3D(cube.X + dx, cube.Y + dy, cube.Z + dz);

                        neighbouringCubes.AddOrUpdate(neighbour, 1, (_, count) => count + 1);
                    }
                }
            }
        }
        
        private static void ProcessActiveHyperCube(Point4D cube, ConcurrentDictionary<Point4D, int> neighbouringHyperCubes)
        {
            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    for (var dz = -1; dz <= 1; dz++)
                    {
                        for (var dw = -1; dw <= 1; dw++)
                        {
                            if (dx == 0 && dy == 0 && dz == 0 && dw == 0)
                            {
                                continue;
                            }
                            
                            var hyperNeighbour = new Point4D(cube.X + dx, cube.Y + dy, cube.Z + dz, cube.W + dw);

                            neighbouringHyperCubes.AddOrUpdate(hyperNeighbour, 1, (_, count) => count + 1);
                        }
                    }
                }
            }
        }
    }
}