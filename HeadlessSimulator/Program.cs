using System;
using System.Globalization;
using System.IO;
using BikeMania.Core;

class Program
{
    static void Main(string[] args)
    {
        var levelPath = args.Length > 0 ? args[0] : Path.Combine("Assets","Resources","Levels","example.json");
        var outPath = args.Length > 1 ? args[1] : "simulation_output.csv";
        var duration = args.Length > 2 ? double.Parse(args[2], CultureInfo.InvariantCulture) : 30.0;
        var dt = args.Length > 3 ? double.Parse(args[3], CultureInfo.InvariantCulture) : 0.02;

        Console.WriteLine($"Level: {levelPath}");
        Console.WriteLine($"Output: {outPath}");
        Console.WriteLine($"Duration: {duration}s, dt={dt}s");

        if (!File.Exists(levelPath))
        {
            Console.Error.WriteLine("Level file not found: " + levelPath);
            return;
        }

        var level = LevelParser.ParseFromFile(levelPath);
        var bike = new BikeModel();
        bike = SetupBikeFromSpawn(bike, level.Spawn);

        var cpManager = new CheckpointManager(level.Checkpoints);

        using var writer = new StreamWriter(outPath);
        writer.WriteLine("time,posX,posY,velX,velY,angle_rad,angularVel,speed,distToNext,next_checkpoint,checkpoints_reached");

        int steps = (int)Math.Ceiling(duration / dt);
        // PID controller state
        double integral = 0.0;
        double prevErr = 0.0;

        for (int i = 0; i < steps; i++)
        {
            double time = i * dt;

            // Simple controller: aim toward next checkpoint if any
            float move = 1.0f; // full throttle by default
            float turn = 0f;

            // Get next checkpoint index by inspecting cpManager via a small reflection of logic
            // We'll approximate by searching the next unreached index using the CheckpointManager behaviour: we cannot read nextIndex, so instead just aim at the first checkpoint that is not yet reached (find the first further than threshold)
            int nextIdx = -1;
            for (int j = 0; j < (level.Checkpoints?.Count ?? 0); j++)
            {
                var cp = level.Checkpoints[j];
                var dx = cp.X - bike.Position.X;
                var dy = cp.Y - bike.Position.Y;
                var dist2 = dx * dx + dy * dy;
                if (dist2 > 1.0f * 1.0f)
                {
                    nextIdx = j;
                    break;
                }
            }

            if (nextIdx >= 0)
            {
                var cp = level.Checkpoints[nextIdx];
                var dx = cp.X - bike.Position.X;
                var dy = cp.Y - bike.Position.Y;
                var desired = Math.Atan2(dy, dx);
                var err = NormalizeAngle(desired - (double)bike.Angle);

                // PID controller for steering
                double Kp = 3.0;
                double Ki = 0.5;
                double Kd = 0.2;
                integral += err * dt;
                var derivative = (err - prevErr) / dt;
                var output = Kp * err + Ki * integral + Kd * derivative;
                prevErr = err;

                turn = (float)Clamp(output, -1.0, 1.0);

                // reduce throttle if steep angle
                var angleDiff = Math.Abs(err);
                if (angleDiff > Math.PI / 2) move = 0.0f;
                else if (angleDiff > Math.PI / 3) move = 0.3f;
            }

            bike.Step(move, turn, (float)dt);

            // Check for reached checkpoints to count them
            int reached = 0;
            if (level.Checkpoints != null)
            {
                foreach (var cp in level.Checkpoints)
                {
                    var dx = cp.X - bike.Position.X;
                    var dy = cp.Y - bike.Position.Y;
                    if (dx * dx + dy * dy <= 1.0f) reached++;
                }
            }

            // telemetry
            double speed = Math.Sqrt(bike.Velocity.X * bike.Velocity.X + bike.Velocity.Y * bike.Velocity.Y);
            double angularVel = bike.AngularVelocity;
            double distToNext = -1.0;
            if (nextIdx >= 0)
            {
                var cp = level.Checkpoints[nextIdx];
                var dx = cp.X - bike.Position.X;
                var dy = cp.Y - bike.Position.Y;
                distToNext = Math.Sqrt(dx * dx + dy * dy);
            }

            writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0:0.000},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",
                time,
                bike.Position.X, bike.Position.Y,
                bike.Velocity.X, bike.Velocity.Y,
                bike.Angle,
                angularVel,
                speed,
                distToNext,
                nextIdx,
                reached
            ));
        }

        Console.WriteLine("Simulation complete. Output written to " + outPath);
    }

    static BikeModel SetupBikeFromSpawn(BikeModel bike, SpawnPoint spawn)
    {
        if (spawn == null) return bike;
        bike = new BikeModel();
        bike.GetType().GetProperty("Position").SetValue(bike, new BikeMania.Core.Vector2(spawn.X, spawn.Y));
        // Angle in spawn is expected in degrees; convert to radians
        var radians = (float)(spawn.Rotation * Math.PI / 180.0);
        bike.GetType().GetProperty("Angle").SetValue(bike, radians);
        return bike;
    }

    static double NormalizeAngle(double a)
    {
        while (a > Math.PI) a -= 2 * Math.PI;
        while (a <= -Math.PI) a += 2 * Math.PI;
        return a;
    }

    static double Clamp(double v, double lo, double hi) => v < lo ? lo : (v > hi ? hi : v);
}
