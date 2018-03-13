using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.Core;
using DogWatch.Types;
using Moq;
using Xunit;

namespace DogWatch.Tests
{
    public class About_tags
    {
        [Fact]
        public void adding_a_tag_appends_it_with_a_hash()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            var colour = new StatTag("colour", "blue");
            metrics.Gauge("mystat", 1, 1D, colour);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|#colour:blue"),
                $@"Expected the tag string: ""|#colour:blue"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void multiple_tags_are_separated_by_comma()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            var colour = new StatTag("colour", "blue");
            var shape = new StatTag("shape", "circle");
            metrics.Gauge("mystat", 1, 1D, colour, shape);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|#colour:blue,shape:circle"),
                $@"Expected the tag string: ""|#colour:blue,shape:circle"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void a_tag_need_not_have_value()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            var isBlue = new StatTag("isBlue");
            var isCircle = new StatTag("isCircle");
            metrics.Gauge("mystat", 1, 1D, isBlue, isCircle);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|#isBlue,isCircle"),
                $@"Expected the tag string: ""|#isBlue,isCircle"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void with_no_tags_it_still_includes_the_hash()
        {
            // I need to check if this is 'ok' by the integration
            // but for now just documenting the behaviour

            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Gauge("mystat", 1);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|#"),
                $@"Expected the tag string: ""|#"" but couldn't find it in <{logs.First()}>");
        }
    }
}