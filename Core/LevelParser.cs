using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace BikeMania.Core
{
    // Simple level representation and parser.
    public class Level
    {
        public SpawnPoint Spawn { get; set; }
        public List<Checkpoint> Checkpoints { get; set; }
        public List<TrackSegment> Track { get; set; }
    }

    public class SpawnPoint { public float X { get; set; } public float Y { get; set; } public float Rotation { get; set; } }
    public class Checkpoint { public float X { get; set; } public float Y { get; set; } }
    public class TrackSegment { public float X { get; set; } public float Y { get; set; } public float Width { get; set; } public float Height { get; set; } public string Type { get; set; } }

    public static class LevelParser
    {
        public static Level ParseFromJson(string json)
        {
            return JsonConvert.DeserializeObject<Level>(json);
        }

        public static Level ParseFromFile(string path)
        {
            var txt = File.ReadAllText(path);
            return ParseFromJson(txt);
        }
    }
}
