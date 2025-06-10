using System;

namespace AICO.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing a URL
    /// </summary>
    public sealed record Url
    {
        /// <summary>
        /// The URL value
        /// </summary>
        public string Value { get; }
        
        private Url(string value)
        {
            Value = value;
        }
        
        /// <summary>
        /// Creates a new URL value object
        /// </summary>
        /// <param name="value">Raw URL string</param>
        /// <returns>URL value object</returns>
        /// <exception cref="ArgumentException">Thrown when URL is invalid</exception>
        public static Url Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("URL cannot be empty", nameof(value));
                
            if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) || 
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                throw new ArgumentException("Invalid URL format. Must be a valid HTTP or HTTPS URL.", nameof(value));
                
            return new Url(value.Trim());
        }
        
        public override string ToString() => Value;
    }
}