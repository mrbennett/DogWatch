using System;
using System.Globalization;
using System.Linq;
using Amazon.Lambda.Core;
using DogWatch.Types;

namespace DogWatch
{
    public class DogMetrics : IMetrics
    {
        //TODO: Common tags?

        private const string CounterStatType = "count";
        private const string GaugeStatType = "gauge";
        private const string HistogramStatType = "histogram";
        private const string CheckStatType = "check";

        private readonly Random _random = new Random();
        private readonly ILambdaLogger _logger;

        public DogMetrics(ILambdaLogger logger)
        {
            _logger = logger;
        }

        private void LogMetric(string statName, string statType, string value, double sampleRate, params StatTag[] tags)
        {
            if (_random.NextDouble() < sampleRate)
            {
                var tagStrings = tags.Select(tag => tag.ToString());
                var tagString = $"#{string.Join(",", tagStrings)}";

                _logger.LogLine(
                    $"MONITORING|{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}|{value}|{statType}|{statName}|{tagString}");
            }
        }

        public void Counter(string statName, long value, double sampleRate = 1, params StatTag[] tags)
        {
            LogMetric(statName, CounterStatType, value.ToString(), sampleRate, tags);
        }

        public void Gauge(string statName, double value, double sampleRate = 1, params StatTag[] tags)
        {
            LogMetric(statName, GaugeStatType, value.ToString(CultureInfo.InvariantCulture), sampleRate, tags);
        }

        public void Histogram(string statName, double value, double sampleRate = 1, params StatTag[] tags)
        {
            LogMetric(statName, HistogramStatType, value.ToString(CultureInfo.InvariantCulture), sampleRate, tags);
        }

        public void Check(string statName, ServiceCheck value, double sampleRate = 1, params StatTag[] tags)
        {
            var enumVal = (int) value;

            LogMetric(statName, CheckStatType, enumVal.ToString(), sampleRate, tags);
        }
    }
}
