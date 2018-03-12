using System;
using Amazon.Lambda.Core;
using DogWatch.Types;

namespace DogWatch
{
    public class DogMetrics : IMetrics
    {
        private readonly ILambdaLogger _logger;

        public DogMetrics(ILambdaLogger logger)
        {
            _logger = logger;
        }

        public void Counter(string statName, long value, double sampleRate = 1, params StatTag[] tags)
        {
            _logger.LogLine($"MONITORING|{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}|{value}|count|{statName}");
        }

        public void Increment(string statName, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Decrement(string statName, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Gauge(string statName, double value, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Histogram(string statName, double value, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        //TODO: Could the methods from here on be extensions?

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
