namespace DogWatch.Types
{
    public class StatTag
    {
        private readonly string _name;
        private readonly object _value;

        public StatTag(string name, object value = null)
        {
            _name = name;
            _value = value;
        }

        public override string ToString()
            => _value != null
                ? $"{_name}:{_value}"
                : $"{_name}";
    }
}