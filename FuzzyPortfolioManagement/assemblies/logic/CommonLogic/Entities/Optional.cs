using System;

namespace CommonLogic.Entities
{
    public sealed class Optional<T> where T : class
    {
        private readonly T _value;

        private Optional(T value)
        {
            _value = value;
        }

        public static Optional<T> Empty() => new Optional<T>(null);

        public static Optional<T> For(T value) => new Optional<T>(value);

        public bool IsPresent => _value != null;

        public T Value
        {
            get
            {
                if (_value == null) throw new ArgumentNullException(nameof(Value));
                return _value;
            }
        }
    }
}