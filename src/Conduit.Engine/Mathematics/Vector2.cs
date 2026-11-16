using Silk.NET.Maths;
using System.Diagnostics.CodeAnalysis;

namespace Conduit.Engine.Mathematics
{
    public struct Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return this.X;
                    case 1: return this.Y;
                    default: throw new IndexOutOfRangeException("Invalid Vector2 index");
                }
            }
            set
            {
                switch (index)
                {
                    case 0: this.X = value; break;
                    case 1: this.Y = value; break;
                    default: throw new IndexOutOfRangeException("Invalid Vector2 index");
                }
            }
        }

        public float Length => (float)Math.Sqrt(X * X + Y * Y);
        public Vector2 Normalized => this / Length;
        public float Dot(Vector2 other) => X * other.X + Y * other.Y;
        public float Distance(Vector2 other) => (other - this).Length;

        // Vector addition
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        // Vector subtraction
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        // Scalar multiplication
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.X * b, a.Y * b);
        }

        // Scalar multiplication
        public static Vector2 operator *(float a, Vector2 b)
        {
            return new Vector2(b.X * a, b.Y * a);
        }

        // Scalar division
        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(a.X / b, a.Y / b);
        }

        // Component-wise multiplication
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }

        // Component-wise division
        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }

        // Equality comparison
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        // Inequality comparison
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public static float Dot(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;
        public static float Distance(Vector2 a, Vector2 b) => (b - a).Length;

        // Predefined vectors
        public static Vector2 Zero => new Vector2(0, 0);
        public static Vector2 One => new Vector2(1, 1);
        public static Vector2 MinusOne => new Vector2(-1, -1);
        public static Vector2 Infinity => new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        public static Vector2 Up => new Vector2(0, 1);
        public static Vector2 Down => new Vector2(0, -1);
        public static Vector2 Right => new Vector2(1, 0);
        public static Vector2 Left => new Vector2(-1, 0);

        // Conversions to System.Numerics.Vector2
        public static explicit operator System.Numerics.Vector2(Vector2 v) => new System.Numerics.Vector2(v.X, v.Y);
        public static explicit operator Vector2(System.Numerics.Vector2 v) => new Vector2(v.X, v.Y);

        // Conversions to Silk.NET.Maths.Vector2D<float>
        public static explicit operator Silk.NET.Maths.Vector2D<float>(Vector2 v) => new Silk.NET.Maths.Vector2D<float>(v.X, v.Y);
        public static explicit operator Vector2(Silk.NET.Maths.Vector2D<float> v) => new Vector2(v.X, v.Y);

        public override string ToString() => $"({X}, {Y})";
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return obj is Vector2 vec && this == vec;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

}
