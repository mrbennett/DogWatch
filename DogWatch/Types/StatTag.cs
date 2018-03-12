namespace DogWatch.Types
{
    public class StatTag
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            return $"{Name}:{Value}";
        }
    }
}