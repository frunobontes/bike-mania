using NUnit.Framework;
using BikeMania.Core;
using System.Collections.Generic;

namespace BikeMania.Core.Tests
{
    public class CheckpointManagerTests
    {
        [Test]
        public void Reaches_Checkpoints_In_Order()
        {
            var cps = new List<Checkpoint> { new Checkpoint { X = 1, Y = 0 }, new Checkpoint { X = 2, Y = 0 } };
            var mgr = new CheckpointManager(cps);

            Assert.IsFalse(mgr.AllReached);
            Assert.IsTrue(mgr.IsNextCheckpointReached(1, 0, 0.1f));
            Assert.IsFalse(mgr.AllReached);
            Assert.IsTrue(mgr.IsNextCheckpointReached(2, 0, 0.1f));
            Assert.IsTrue(mgr.AllReached);
        }

        [Test]
        public void DoesNot_Advance_When_OutOfRange()
        {
            var cps = new List<Checkpoint> { new Checkpoint { X = 10, Y = 0 } };
            var mgr = new CheckpointManager(cps);

            Assert.IsFalse(mgr.IsNextCheckpointReached(0, 0, 1f));
            Assert.IsFalse(mgr.AllReached);
        }
    }
}
