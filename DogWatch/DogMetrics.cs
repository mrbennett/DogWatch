using System;
using DogWatch.Types;

namespace DogWatch
{
    public class DogMetrics : IMetrics
    {
        public void Counter<T>(string statName, T value, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Increment(string statName, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Decrement(string statName, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Gauge<T>(string statName, T value, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Histogram<T>(string statName, T value, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Set<T>(string statName, T value, double sampleRate = 1, params StatTag[] tags)
        {
            throw new NotImplementedException();
        }

        public void Timer<T>(string statName, T value, double sampleRate = 1, params StatTag[] tags)
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
