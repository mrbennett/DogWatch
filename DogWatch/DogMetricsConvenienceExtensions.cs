using System;
using System.Diagnostics;
using DogWatch.Types;

namespace DogWatch
{
    public static class DogMetricsConvenienceExtensions
    {
        public static void Increment(this IMetrics metrics, string statName, double sampleRate = 1, params StatTag[] tags) 
            => metrics.Counter(statName, 1, sampleRate, tags);

        public static void Decrement(this IMetrics metrics, string statName, double sampleRate = 1, params StatTag[] tags)
            => metrics.Counter(statName, -1, sampleRate, tags);

        public static void Timer(this IMetrics metrics, string statName, double value, double sampleRate = 1, params StatTag[] tags)
            => metrics.Histogram(statName, value, sampleRate, tags);

        public static T Time<T>(this IMetrics metrics, Func<T> func, string statName, double sampleRate = 1, params StatTag[] tags)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = func();
            stopwatch.Stop();

            metrics.Timer(statName, stopwatch.ElapsedMilliseconds, sampleRate, tags);
            return result;
        }

        public static IDisposable StartTimer(this IMetrics metrics, string statName, double sampleRate = 1, params StatTag[] tags)
        {
            return new MetricsTimer(value => metrics.Timer(statName, value, sampleRate, tags));
        }

        private class MetricsTimer : IDisposable
        {
            private readonly Action<double> _recordTime;
            private readonly Stopwatch _stopwatch;

            public MetricsTimer(Action<double> recordTime)
            {
                _recordTime = recordTime;
                _stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                _stopwatch.Stop();
                _recordTime(_stopwatch.ElapsedMilliseconds);
            }
        }
    }
}