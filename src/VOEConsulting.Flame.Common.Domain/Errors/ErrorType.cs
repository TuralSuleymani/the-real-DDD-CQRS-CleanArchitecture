using Ardalis.SmartEnum;

namespace VOEConsulting.Flame.Common.Domain.Errors
{
    public abstract class ErrorType(string name, int value) : SmartEnum<ErrorType>(name, value)
    {
        public static readonly ErrorType Conflict = new ConflictEnum();
        public static readonly ErrorType NotFound = new NotFoundEnum();
        public static readonly ErrorType BadRequest = new BadRequestEnum();
        public static readonly ErrorType Validation = new ValidationEnum();
        public static readonly ErrorType Unexpected = new UnexpectedEnum();

        // Define each specific ErrorType as a nested class that extends SmartEnum
        private class ConflictEnum : ErrorType
        {
            public ConflictEnum() : base("Conflict", 0) { }
        }

        private class NotFoundEnum : ErrorType
        {
            public NotFoundEnum() : base("NotFound", 1) { }
        }

        private class BadRequestEnum : ErrorType
        {
            public BadRequestEnum() : base("BadRequest", 2) { }
        }

        private class ValidationEnum : ErrorType
        {
            public ValidationEnum() : base("Validation", 3) { }
        }
        private class UnexpectedEnum : ErrorType
        {
            public UnexpectedEnum() : base("Unexpected", 4) { }
        }
    }

}
