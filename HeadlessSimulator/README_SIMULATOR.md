Headless Simulator

This console app runs the Core library in a headless (non-Unity) mode and writes telemetry to CSV.

How to run
1. Ensure .NET 6.0+ SDK is installed.
2. From the project root (C:\\Users\\Bontes\\Documents\\antigravity\\Bike Mania) run:
   dotnet run --project HeadlessSimulator -- Assets/Resources/Levels/example.json simulation_output.csv 30 0.02

Arguments
- [levelPath] (optional) - path to level JSON (default: Assets/Resources/Levels/example.json)
- [outCsv] (optional) - output CSV file (default: simulation_output.csv)
- [duration] (optional) - seconds of simulation (default: 30)
- [dt] (optional) - timestep in seconds (default: 0.02)

The simulator steers the bike toward the next checkpoint and records time, position, velocity, angle, next checkpoint index, and number of checkpoints reached.
