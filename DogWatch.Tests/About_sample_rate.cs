using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Lambda.Core;
using DogWatch.Types;
using Moq;
using Xunit;

namespace DogWatch.Tests
{
    public class About_sample_rate
    {
        [Fact]
        public void sample_rate_of_1_always_sends_it()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            Repeat(100, () =>
            {
                var metrics = new DogMetrics(contextMock.Object);
                metrics.Gauge("mystat", 1, 1D); //don't usually need to specify 1D
            });

            Assert.True(logs.Count == 100,
                $"Expected all lines to come through but only had {logs.Count}");
        }

        [Fact]
        public void sample_rate_of_0_never_sends_it()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            Repeat(100, () =>
            {
                var metrics = new DogMetrics(contextMock.Object);
                metrics.Gauge("mystat", 1, 0D);
            });

            Assert.True(logs.Count == 0,
                $"Expected no lines to come through but had {logs.Count}");
        }

        [Fact]
        public void a_sample_rate_of_0_point_5_sends_it_roughly_half_the_time()
        {
            // You probably shouldn't use this for .Counter or .Check though
            // since there is no way to tell datadog it is sampled...

            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            Repeat(100, () =>
            {
                var metrics = new DogMetrics(contextMock.Object);
                metrics.Gauge("mystat", 1, 0.5D);
            });

            Assert.InRange(logs.Count, 40, 60);
        }

        private void Repeat(int times, Action action)
        {
            foreach (var _ in Enumerable.Range(0, times))
            {
                action();
            }
        }
    }
}