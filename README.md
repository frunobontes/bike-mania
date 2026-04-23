<!-- README updated with Core instructions -->

Bike Mania - Prototype

Core development workflow (no Unity required):

1. The Core/ folder contains a netstandard2.0 library with game logic (BikeModel, Level parser, GameRules).
2. The Core.Tests/ folder contains NUnit tests. To run them:
   - Make sure .NET SDK (6.0+) is installed.
   - From this folder run: dotnet test "Core.Tests\\BikeMania.Core.Tests.csproj"
3. To integrate with Unity: compile the Core project to a DLL (dotnet build) and copy the generated DLL from bin/ to Assets/Plugins/, or copy the source files into Assets/Plugins/Core/.

Running tests locally
- If you have .NET SDK installed: run `dotnet test "Core.Tests/BikeMania.Core.Tests.csproj"` from the "Bike Mania" folder.
- If you do not want to install the SDK, use Docker (requires Docker installed): run `.
  ci/run-tests.ps1` (PowerShell) which builds an image and runs the tests.

Files added:
- Core/ (C# library)
- Core.Tests/ (NUnit tests)
- Assets/Resources/Levels/example.json
- Assets/Sprites/PLACEHOLDER_README.txt

Follow-up options:
- I can create a command-line harness to run headless simulations using the Core library.
- I can create Unity glue code (a small wrapper) that loads example.json and instantiates GameObjects.

What I added next (simulator, visualizer, Unity glue)
- HeadlessSimulator/: console app that runs simulations using Core and writes CSV telemetry.
- Tools/Visualizer/visualizer.html: simple browser visualizer that reads the CSV and animates the bike.
- Assets/UnityGlue/: LevelLoader and CameraFollow scripts so Unity can load example.json and show primitives.

Quick usage
1. Run headless simulation and write CSV:
   dotnet run --project HeadlessSimulator -- Assets/Resources/Levels/example.json simulation_output.csv 30 0.02
2. Open Tools/Visualizer/visualizer.html in your browser and load simulation_output.csv.
3. Open the folder in Unity for visual tests; add the LevelLoader component to an empty GameObject and set a player prefab (or create a simple sprite GameObject named player) and optionally a ground material.
