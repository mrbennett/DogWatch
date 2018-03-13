using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.Core;
using DogWatch.Types;
using Moq;
using Xunit;

namespace DogWatch.Tests
{
    public class About_check
    {
        [Fact]
        public void it_contains_the_monitoring_command_string()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                       .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Check("mystat", ServiceCheck.Ok);

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
            metrics.Check("mystat", ServiceCheck.Ok);

            var currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains($"|{currentUnixTime}|"),
                $@"Expected the current unix timestamp: ""|{currentUnixTime}|"" but couldn't find it in <{logs.First()}>");
        }

        [Theory]
        [InlineData(ServiceCheck.Ok, 0)]
        [InlineData(ServiceCheck.Warning, 1)]
        [InlineData(ServiceCheck.Critical, 2)]
        [InlineData(ServiceCheck.Unknown, 3)]
        public void it_contains_the_given_value(ServiceCheck checkValue, int expectedValue)
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                       .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Check("mystat", checkValue);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains($"|{expectedValue}|"),
                $@"Expected the given stat value: ""|{expectedValue}|"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void it_contains_the_stat_type()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                       .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Check("mystat", ServiceCheck.Ok);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|check|"),
                $@"Expected the given stat type: ""|check|"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void it_contains_the_stat_name()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                       .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Check("mystat", ServiceCheck.Ok);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|mystat|"),
                $@"Expected the given stat name: ""|mystat|"" but couldn't find it in <{logs.First()}>");
        }
    }
}