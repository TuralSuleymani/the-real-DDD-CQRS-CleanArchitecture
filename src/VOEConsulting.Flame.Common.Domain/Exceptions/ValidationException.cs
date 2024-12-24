namespace VOEConsulting.Flame.Common.Domain.Exceptions
{

    [Serializable]
    public class ValidationException : Exception
    {
        public IReadOnlyList<string> Errors { get; }

        // Constructor to initialize with multiple errors
        public ValidationException(IEnumerable<string> errors)
            : base("Validation failed.")
        {
            Errors = errors?.ToList() ?? new List<string>();
        }

        // Constructor to initialize with a single error
        public ValidationException(string error)
            : base(error)
        {
            Errors = new List<string>();
        }

        // Constructor to provide a custom message and an inner exception
        public ValidationException(string message, Exception inner)
            : base(message, inner)
        {
            Errors = new List<string>() { inner.Message }; // Default to an empty list
        }

        // Serialization constructor
        protected ValidationException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            Errors = (IReadOnlyList<string>)info.GetValue(nameof(Errors), typeof(IReadOnlyList<string>))
                     ?? new List<string>();
        }

        // Override GetObjectData to serialize the Errors property
        public override void GetObjectData(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Errors), Errors, typeof(IReadOnlyList<string>));
        }
    }

}
