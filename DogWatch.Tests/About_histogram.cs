using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.Core;
using Moq;
using Xunit;

namespace DogWatch.Tests
{
    public class About_histogram
    {
        [Fact]
        public void it_contains_the_monitoring_command_string()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                       .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Histogram("mystat", 1);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("MONITORING"),
                $@"Expected the command string: ""MONITORING"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void it_contains_the_current_unix_timestamp()
        {
            // Could this test fail when run on exactly the border of a second?

            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                       .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Histogram("mystat", 1);

            var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains($"|{currentUnixTime}|"),
                $@"Expected the current unix timestamp: ""|{currentUnixTime}|"" but couldn't find it in <{logs.First()}>");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(-1)]
        [InlineData(0)]
        public void it_contains_the_given_value(long value)
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                       .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Histogram("mystat", value);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains($"|{value}|"),
                $@"Expected the given stat value: ""|{value}|"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void it_contains_the_stat_type()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                       .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Histogram("mystat", 1);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|histogram|"),
                $@"Expected the given stat type: ""|histogram|"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void it_contains_the_stat_name()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                       .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Histogram("mystat", 1);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|mystat|"),
                $@"Expected the given stat name: ""|mystat|"" but couldn't find it in <{logs.First()}>");
        }
    }
}