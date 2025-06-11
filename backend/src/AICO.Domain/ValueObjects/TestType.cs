using System;
using System.Collections.Generic;
using System.Linq;

namespace AICO.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing A/B test types
    /// </summary>
    public class TestType : IEquatable<TestType>
    {
        public static readonly TestType Headline = new("Headline");
        public static readonly TestType CallToAction = new("CTA");
        public static readonly TestType Layout = new("Layout");
        public static readonly TestType Image = new("Image");
        public static readonly TestType Button = new("Button");
        public static readonly TestType Form = new("Form");
        public static readonly TestType Content = new("Content");

        private static readonly List<TestType> _predefinedTypes = new()
        {
            Headline, CallToAction, Layout, Image, Button, Form, Content
        };

        public string Value { get; }

        private TestType(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Creates a custom test type
        /// </summary>
        public static TestType Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Test type value cannot be null or empty", nameof(value));

            // Check if it's a predefined type first
            var predefined = _predefinedTypes.FirstOrDefault(t => 
                string.Equals(t.Value, value, StringComparison.OrdinalIgnoreCase));
            
            return predefined ?? new TestType(value);
        }

        /// <summary>
        /// Gets all predefined test types
        /// </summary>
        public static IReadOnlyList<TestType> GetPredefinedTypes() => _predefinedTypes.AsReadOnly();

        public bool Equals(TestType other) => 
            other != null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object obj) => Equals(obj as TestType);

        public override int GetHashCode() => 
            Value?.ToLowerInvariant().GetHashCode() ?? 0;

        public override string ToString() => Value;

        public static implicit operator string(TestType testType) => testType?.Value;

        public static explicit operator TestType(string value) => Create(value);
    }
}