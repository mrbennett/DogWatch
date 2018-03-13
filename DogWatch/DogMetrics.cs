using System;
using System.Globalization;
using Amazon.Lambda.Core;
using DogWatch.Types;

namespace DogWatch
{
    public class DogMetrics : IMetrics
    {
        private const string CounterStatType = "count";
        private const string GaugeStatType = "gauge";
        private const string HistogramStatType = "histogram";
        private const string CheckStatType = "check";


        private readonly ILambdaLogger _logger;

        public DogMetrics(ILambdaLogger logger)
        {
            _logger = logger;
        }

        private void LogMetric(string statName, string statType, string value)
        {
            _logger.LogLine($"MONITORING|{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}|{value}|{statType}|{statName}|");
        }

        public void Counter(string statName, long value, double sampleRate = 1, params StatTag[] tags)
        {
            LogMetric(statName, CounterStatType, value.ToString());
        }

        public void Gauge(string statName, double value, double sampleRate = 1, params StatTag[] tags)
        {
            LogMetric(statName, GaugeStatType, value.ToString(CultureInfo.InvariantCulture));
        }

        public void Histogram(string statName, double value, double sampleRate = 1, params StatTag[] tags)
        {
            LogMetric(statName, HistogramStatType, value.ToString(CultureInfo.InvariantCulture));
        }

        public void Check(string statName, ServiceCheck value, double sampleRate = 1, params StatTag[] tags)
        {
            var enumVal = (int) value;

            LogMetric(statName, CheckStatType, enumVal.ToString());
        }

        //TODO: Could the methods from here on be extensions?
        public void Increment(string statName, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Decrement(string statName, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Timer(string statName, double value, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public IDisposable StartTimer(string name, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public T Time<T>(Func<T> func, string statName, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }
    }
}
