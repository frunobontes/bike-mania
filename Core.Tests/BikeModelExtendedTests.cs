using NUnit.Framework;
using BikeMania.Core;

namespace BikeMania.Core.Tests
{
    public class BikeModelExtendedTests
    {
        [Test]
        public void Step_WithZeroDt_DoesNotChangeState()
        {
            var bike = new BikeModel();
            var pos = bike.Position;
            var vel = bike.Velocity;
            var ang = bike.Angle;

            bike.Step(1f, 1f, 0f);

            Assert.AreEqual(pos.X, bike.Position.X);
            Assert.AreEqual(pos.Y, bike.Position.Y);
            Assert.AreEqual(vel.X, bike.Velocity.X);
            Assert.AreEqual(vel.Y, bike.Velocity.Y);
            Assert.AreEqual(ang, bike.Angle);
        }

        [Test]
        public void Throttle_Then_NoThrottle_SlowsDown()
        {
            var bike = new BikeModel();

            // Give a short burst of throttle
            bike.Step(1f, 0f, 0.1f);
            bike.Step(1f, 0f, 0.1f);

            var speedAfterBurst = System.Math.Sqrt(bike.Velocity.X * bike.Velocity.X + bike.Velocity.Y * bike.Velocity.Y);

            // Then let it coast without throttle
            bike.Step(0f, 0f, 0.5f);
            bike.Step(0f, 0f, 0.5f);

            var speedAfterCoast = System.Math.Sqrt(bike.Velocity.X * bike.Velocity.X + bike.Velocity.Y * bike.Velocity.Y);

            Assert.Less(speedAfterCoast, speedAfterBurst + 1e-6);
        }

        [Test]
        public void Multiple_Steps_Increase_Position_When_Throttle()
        {
            var bike = new BikeModel();
            var initialX = bike.Position.X;

            for (int i = 0; i < 10; i++)
                bike.Step(1f, 0f, 0.1f);

            Assert.Greater(bike.Position.X, initialX);
        }
    }
}
