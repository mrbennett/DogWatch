using System;
using DogWatch.Types;

namespace DogWatch
{
    public interface IMetrics
    {
        void Counter<T>(string statName, T value, double sampleRate = 1.0, params StatTag[] tags);

        void Increment(string statName, double sampleRate = 1.0, params StatTag[] tags);

        void Decrement(string statName, double sampleRate = 1.0, params StatTag[] tags);

        void Gauge<T>(string statName, T value, double sampleRate = 1.0, params StatTag[] tags);

        void Histogram<T>(string statName, T value, double sampleRate = 1.0, params StatTag[] tags);

        void Set<T>(string statName, T value, double sampleRate = 1.0, params StatTag[] tags);

        void Timer<T>(string statName, T value, double sampleRate = 1.0, params StatTag[] tags);

        IDisposable StartTimer(string name, double sampleRate = 1.0, params StatTag[] tags);

        T Time<T>(Func<T> func, string statName, double sampleRate = 1.0, params StatTag[] tags);
    }
}