using System.Numerics;
using Vector2 = Conduit.Engine.Mathematics.Vector2;

namespace Conduit.Engine.Components
{
    public sealed class Transform : Component
    {
        // Using ref returns to allow modyfying individual fields of value types directly
        // (e.g. Position.X = 10) without needing to reassign the entire value

        private Vector2 _position = Vector2.Zero;
        public ref Vector2 Position => ref _position;

        private Vector2 _scale = Vector2.One;
        public ref Vector2 Scale => ref _scale;

        public float Rotation { get; set; } = 0f;

        public Transform(Vector2 position, Vector2 scale, float rotation)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation;
        }

        public Transform() { }

        public Matrix4x4 GetModelMatrix() => 
            Matrix4x4.CreateTranslation(Position.X, Position.Y, 0) *
            Matrix4x4.CreateRotationZ(Rotation * (MathF.PI / 180f)) *
            Matrix4x4.CreateScale(Scale.X, Scale.Y, 1);
    }
}
