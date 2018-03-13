using System;
using DogWatch.Types;

namespace DogWatch
{
    public interface IMetrics
    {
        void Counter(string statName, long value, double sampleRate = 1.0, params StatTag[] tags);

        void Increment(string statName, double sampleRate = 1.0, params StatTag[] tags);

        void Decrement(string statName, double sampleRate = 1.0, params StatTag[] tags);

        void Gauge(string statName, double value, double sampleRate = 1.0, params StatTag[] tags);

        void Histogram(string statName, double value, double sampleRate = 1.0, params StatTag[] tags);

        void Check(string statName, ServiceCheck value, double sampleRate = 1.0, params StatTag[] tags);

        void Timer(string statName, double value, double sampleRate = 1.0, params StatTag[] tags);

        IDisposable StartTimer(string name, double sampleRate = 1.0, params StatTag[] tags);

        T Time<T>(Func<T> func, string statName, double sampleRate = 1.0, params StatTag[] tags);
    }
}