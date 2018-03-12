using System;

namespace SmartValley.Domain.Core
{
    public struct Address : IEquatable<Address>
    {
        private readonly string _value;

        public Address(string value)
        {
            _value = value.ToLower();
        }

        public static implicit operator string(Address address)
        {
            return address._value.ToLower();
        }

        public static implicit operator Address(string address)
        {
            return new Address(address.ToLower());
        }

        public override string ToString()
        {
            return _value.ToLower();
        }

        public static bool operator ==(Address x, Address y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Address x, Address y)
        {
            return !x.Equals(y);
        }

        public bool Equals(Address other)
        {
            return string.Equals(_value, other._value, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            return obj is Address && Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            return (_value != null ? _value.GetHashCode() : 0);
        }

        public bool IsEmpty() => string.IsNullOrWhiteSpace(_value?.Replace("0x", "").Replace("0", ""));
    }
}