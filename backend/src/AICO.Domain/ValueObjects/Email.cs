using System.Net.Mail;

namespace AICO.Domain.ValueObjects
{
    /// <summary>
    /// Value object representing an email address
    /// </summary>
    public sealed record Email
    {
        /// <summary>
        /// The email address value
        /// </summary>
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new Email value object
        /// </summary>
        /// <param name="value">Raw email string</param>
        /// <returns>Email value object</returns>
        /// <exception cref="ArgumentException">Thrown when email is invalid</exception>
        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty", nameof(value));

            try
            {
                var mailAddress = new MailAddress(value);
                return new Email(value.Trim().ToLowerInvariant());
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid email format", nameof(value));
            }
        }

        public override string ToString() => Value;
    }
}