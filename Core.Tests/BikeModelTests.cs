using NUnit.Framework;
using BikeMania.Core;

namespace BikeMania.Core.Tests
{
    public class BikeModelTests
    {
        [Test]
        public void Throttle_Increases_Forward_Speed()
        {
            var bike = new BikeModel();
            var initialSpeed = bike.Velocity; // (0,0)
            bike.Step(1f, 0f, 0.1f);
            Assert.IsTrue(bike.Velocity.X > initialSpeed.X || bike.Velocity.Y > initialSpeed.Y);
        }

        [Test]
        public void Turning_Changes_Angle()
        {
            var bike = new BikeModel();
            var initialAngle = bike.Angle;
            bike.Step(0f, 1f, 0.1f);
            Assert.AreNotEqual(initialAngle, bike.Angle);
        }
    }
}
