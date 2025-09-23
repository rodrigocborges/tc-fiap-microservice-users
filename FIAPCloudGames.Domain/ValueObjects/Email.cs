using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FIAPCloudGames.Domain.ValueObjects
{
    [Owned]
    public class Email
    {
        public string Value { get; private set; }

        private Email() { } //EF exige para dar bind nesse value object

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.");

            Value = value;
        }

        public override bool Equals(object? obj) => obj is Email other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}
