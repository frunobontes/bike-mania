using System.Collections.Generic;

namespace BikeMania.Core
{
    public class CheckpointManager
    {
        private readonly List<Checkpoint> checkpoints;
        private int nextIndex = 0;

        public CheckpointManager(List<Checkpoint> checkpoints)
        {
            this.checkpoints = checkpoints ?? new List<Checkpoint>();
            nextIndex = 0;
        }

        public bool IsNextCheckpointReached(float x, float y, float threshold = 1.0f)
        {
            if (nextIndex >= checkpoints.Count) return false;
            var cp = checkpoints[nextIndex];
            var dx = cp.X - x;
            var dy = cp.Y - y;
            var dist2 = dx * dx + dy * dy;
            if (dist2 <= threshold * threshold)
            {
                nextIndex++;
                return true;
            }
            return false;
        }

        public bool AllReached => nextIndex >= checkpoints.Count;
    }
}
