using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Amazon.Lambda.Core;
using Moq;
using Xunit;

namespace DogWatch.Tests
{
    public class About_the_extensions
    {
        [Fact]
        public void increment_is_a_counter_with_value_1()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Increment("mystat");

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|count|"),
                $@"Expected the stat type: ""|count|"" but couldn't find it in <{logs.First()}>");
            Assert.True(logs.First().Contains("|mystat|"),
                $@"Expected the stat name: ""|mystat|"" but couldn't find it in <{logs.First()}>");
            Assert.True(logs.First().Contains("|1|"),
                $@"Expected the stat value: ""|1|"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void decrement_is_a_counter_with_value_negative_1()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Decrement("mystat");

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|count|"),
                $@"Expected the stat type: ""|count|"" but couldn't find it in <{logs.First()}>");
            Assert.True(logs.First().Contains("|mystat|"),
                $@"Expected the stat name: ""|mystat|"" but couldn't find it in <{logs.First()}>");
            Assert.True(logs.First().Contains("|-1|"),
                $@"Expected the stat value: ""|-1|"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void timer_is_just_a_histogram_with_the_value_nothing_fancy()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            metrics.Timer("mystat", 34D);

            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|histogram|"),
                $@"Expected the stat type: ""|histogram|"" but couldn't find it in <{logs.First()}>");
            Assert.True(logs.First().Contains("|mystat|"),
                $@"Expected the stat name: ""|mystat|"" but couldn't find it in <{logs.First()}>");
            Assert.True(logs.First().Contains("|34|"),
                $@"Expected the stat value: ""|34|"" but couldn't find it in <{logs.First()}>");
        }

        [Fact]
        public void time_runs_the_function_and_returns_its_result_plus_logs_a_timer_metric()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            var metrics = new DogMetrics(contextMock.Object);
            var result = metrics.Time(() =>
            {
                Thread.Sleep(3000);
                return "zap";
            }, "mystat");

            Assert.True(result == "zap",
                $@"Expected the result of the function ""zap"" but had <{result}>");
            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|histogram|"),
                $@"Expected the stat type: ""|histogram|"" but couldn't find it in <{logs.First()}>");
            Assert.True(logs.First().Contains("|mystat|"),
                $@"Expected the stat name: ""|mystat|"" but couldn't find it in <{logs.First()}>");
            Assert.InRange(GetTimeFromLog(logs.First()), 3000, 3005);
        }

        private static int GetTimeFromLog(string log) 
            => int.Parse(new Regex(@"\|([0-9]{4})\|").Match(log).Groups[1].Value);

        [Fact]
        public void if_the_given_func_throws_an_exception_then_so_does_time()
        {
            var metrics = new DogMetrics(Mock.Of<ILambdaLogger>());

            Assert.Throws<Exception>(() =>
            {
                metrics.Time<string>(() => throw new Exception("pow!"), "mystat");
            });
        }

        [Fact]
        public void start_timer_runs_the_code_in_the_using_block_plus_logs_a_timer_metric()
        {
            var logs = new List<string>();
            var contextMock = new Mock<ILambdaLogger>(MockBehavior.Strict);
            contextMock.Setup(logger => logger.LogLine(It.IsAny<string>()))
                .Callback((string s) => logs.Add(s));

            int counter = 0;
            var metrics = new DogMetrics(contextMock.Object);
            using (metrics.StartTimer("mystat"))
            {
                counter++;
                Thread.Sleep(3000);
            }
            
            Assert.True(counter == 1,
                "Expected the code in the using block to be executed, but it wasn\'t");
            Assert.True(logs.Count == 1);
            Assert.True(logs.First().Contains("|histogram|"),
                $@"Expected the stat type: ""|histogram|"" but couldn't find it in <{logs.First()}>");
            Assert.True(logs.First().Contains("|mystat|"),
                $@"Expected the stat name: ""|mystat|"" but couldn't find it in <{logs.First()}>");
            Assert.InRange(GetTimeFromLog(logs.First()), 3000, 3005);
        }
    }
}