using System;
using DogWatch.Types;

namespace DogWatch
{
    public interface IMetrics
    {
        void Counter(string statName, long value, double sampleRate = 1.0, params StatTag[] tags);

        void Gauge(string statName, double value, double sampleRate = 1.0, params StatTag[] tags);

        void Histogram(string statName, double value, double sampleRate = 1.0, params StatTag[] tags);

        void Check(string statName, ServiceCheck value, double sampleRate = 1.0, params StatTag[] tags);
    }
}