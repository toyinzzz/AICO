namespace AICO.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing an event type
    /// </summary>
    public sealed record EventType
    {
        /// <summary>
        /// Common event types
        /// </summary>
        public static class Common
        {
            public static readonly EventType PageView = new("page_view");
            public static readonly EventType Click = new("click");
            public static readonly EventType FormSubmit = new("form_submit");
            public static readonly EventType FormAbandonment = new("form_abandonment");
            public static readonly EventType Conversion = new("conversion");
            public static readonly EventType Error = new("error");
            public static readonly EventType Custom = new("custom");
        }

        /// <summary>
        /// The event type value
        /// </summary>
        public string Value { get; }

        private EventType(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new EventType value object
        /// </summary>
        /// <param name="value">Raw event type string</param>
        /// <returns>EventType value object</returns>
        /// <exception cref="ArgumentException">Thrown when event type is invalid</exception>
        public static EventType Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Event type cannot be empty", nameof(value));

            return new EventType(value.Trim().ToLowerInvariant());
        }

        public override string ToString() => Value;
    }
}