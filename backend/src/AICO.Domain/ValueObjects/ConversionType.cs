namespace AICO.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing a conversion type
    /// </summary>
    public sealed record ConversionType
    {
        /// <summary>
        /// Common conversion types
        /// </summary>
        public static class Common
        {
            public static readonly ConversionType Purchase = new("purchase");
            public static readonly ConversionType Signup = new("signup");
            public static readonly ConversionType Lead = new("lead");
            public static readonly ConversionType Download = new("download");
            public static readonly ConversionType Subscription = new("subscription");
            public static readonly ConversionType Custom = new("custom");
        }

        /// <summary>
        /// The conversion type value
        /// </summary>
        public string Value { get; }

        private ConversionType(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new ConversionType value object
        /// </summary>
        /// <param name="value">Raw conversion type string</param>
        /// <returns>ConversionType value object</returns>
        /// <exception cref="ArgumentException">Thrown when conversion type is invalid</exception>
        public static ConversionType Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Conversion type cannot be empty", nameof(value));

            return new ConversionType(value.Trim().ToLowerInvariant());
        }

        public override string ToString() => Value;
    }
}