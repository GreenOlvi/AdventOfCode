namespace AoC2019.Utils
{
    public interface IOption<T>
    {
        bool HasValue { get; }
    }

    public static class Option<T>
    {
        public static Some<T> Some(T value) => new Some<T>(value);
        public static None<T> None() => new None<T>();
    }

    public class Some<T> : IOption<T>
    {
        public Some(T value)
        {
            Value = value;
        }

        public T Value { get; }
        public bool HasValue => true;
    }
    
    public class None<T> : IOption<T>
    {
        public None()
        {
        }

        public bool HasValue => false;
    }
}
