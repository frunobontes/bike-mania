using NUnit.Framework;
using BikeMania.Core;

namespace BikeMania.Core.Tests
{
    public class LevelParserTests
    {
        [Test]
        public void Parse_Example_Level()
        {
            var json = "{\"Spawn\":{\"X\":0,\"Y\":0,\"Rotation\":0},\"Checkpoints\":[{\"X\":5,\"Y\":0}],\"Track\":[{\"X\":0,\"Y\":-1,\"Width\":10,\"Height\":1,\"Type\":\"ground\"}]}";
            var level = LevelParser.ParseFromJson(json);
            Assert.IsNotNull(level);
            Assert.IsNotNull(level.Spawn);
            Assert.AreEqual(1, level.Checkpoints.Count);
            Assert.AreEqual(1, level.Track.Count);
        }
    }
}
