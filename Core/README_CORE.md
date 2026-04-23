Bike Mania - Core Library

This folder contains a pure C# core library with game logic you can develop and test without Unity.

How to run tests:
1. Ensure you have the .NET SDK installed (6.0+ recommended).
2. From this folder run: dotnet test ..\Core.Tests

Notes about integration with Unity:
- You can compile this project into a DLL and place it into Assets/Plugins/, or copy the source files into Assets/Plugins/Core/ for quick iteration inside Unity.
