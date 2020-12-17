using System;

namespace _2020.Utils
{
    public class Point4D : IEquatable<Point4D>
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;
        public readonly int W;

        public Point4D(int x, int y, int z, int w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public bool Equals(Point4D other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.X == other.X && this.Y == other.Y && this.Z == other.Z && this.W == other.W;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Point4D) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y, this.Z, this.W);
        }
    }
}