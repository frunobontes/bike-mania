using System;

namespace BikeMania.Core
{
    // A simple deterministic bike physics model suitable for offline testing.
    // Not a replacement for Unity's Rigidbody2D but good for tuning gameplay logic.
    public class BikeModel
    {
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public float Angle { get; private set; } // radians
        public float AngularVelocity { get; private set; }

        public float Mass { get; set; } = 1.0f;
        public float MotorForce { get; set; } = 200f;
        public float Torque { get; set; } = 5f;
        public float Drag { get; set; } = 0.1f;

        public BikeModel()
        {
            Position = new Vector2(0, 0);
            Velocity = new Vector2(0, 0);
            Angle = 0f;
            AngularVelocity = 0f;
        }

        // moveInput: -1..1 (backward/forward), turnInput: -1..1 (left/right)
        public void Step(float moveInput, float turnInput, float dt)
        {
            // Forward direction is along +X in local space
            var forward = new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));

            // Apply motor (simplified)
            var accel = (MotorForce * moveInput) / Mass;
            Velocity += forward * (accel * dt);

            // Apply drag
            Velocity *= (1f / (1f + Drag * dt));

            // Integrate position
            Position += Velocity * dt;

            // Apply torque for rotation
            var angAccel = Torque * turnInput / Mass;
            AngularVelocity += angAccel * dt;

            // Integrate angle
            Angle += AngularVelocity * dt;

            // Simple angular damping
            AngularVelocity *= (1f / (1f + 2f * dt));
        }
    }

    // Minimal 2D vector struct to avoid extra dependencies in the core
    public struct Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2(float x, float y) { X = x; Y = y; }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, float s) => new Vector2(a.X * s, a.Y * s);
        public static Vector2 operator *(float s, Vector2 a) => a * s;
    }
}
