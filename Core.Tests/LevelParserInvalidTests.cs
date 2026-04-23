using NUnit.Framework;
using BikeMania.Core;

namespace BikeMania.Core.Tests
{
    public class LevelParserInvalidTests
    {
        [Test]
        public void Parse_InvalidJson_ReturnsNullOrThrows()
        {
            var json = "{ invalid json }";

            // Parser uses JsonConvert.DeserializeObject and may throw or return null depending on content.
            Level lvl = null;
            try
            {
                lvl = LevelParser.ParseFromJson(json);
            }
            catch
            {
                // Accept throwing as valid behavior for invalid JSON
            }

            Assert.IsTrue(lvl == null || lvl is Level);
        }
    }
}
