#nullable enable

using System;

namespace Deimos.Utils
{
    public readonly struct Indentation : IEquatable<Indentation>, IComparable<Indentation>
    {
        public static Indentation None = new(0);

        public uint Level { get; }

        public Indentation(uint level)
        {
            Level = level;
        }

        public static Indentation Of(int level)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException(nameof(level), "Indentation level cannot be negative.");
            return new Indentation((uint)level);
        }
        public static Indentation Of(uint level) => new(level);

        public Indentation Increase() => new(Level + 1);
        public Indentation Increase(int amount)
        {
            if (amount < 0)
                return Decrease(-amount);
            return new Indentation(Level + (uint)amount);
        }

        public Indentation Decrease() => new(Level > 0 ? Level - 1 : 0);
        public Indentation Decrease(int amount)
        {
            if (amount < 0)
                return Increase(-amount);
            return new Indentation(Level > amount ? Level - (uint)amount : 0);
        }

        public override string ToString() => new(' ', (int)(Level * 4));

        public override bool Equals(object? obj) => obj is Indentation indentation && Equals(indentation);

        public bool Equals(Indentation other) => Level == other.Level;

        public override int GetHashCode() => HashCode.Combine(Level);

        public int CompareTo(Indentation other) => Level.CompareTo(other.Level);

        public static bool operator ==(Indentation left, Indentation right) => left.Level == right.Level;
        public static bool operator !=(Indentation left, Indentation right) => left.Level != right.Level;

        public static bool operator <(Indentation left, Indentation right) => left.Level < right.Level;
        public static bool operator <=(Indentation left, Indentation right) => left.Level <= right.Level;
        public static bool operator >(Indentation left, Indentation right) => left.Level > right.Level;
        public static bool operator >=(Indentation left, Indentation right) => left.Level >= right.Level;

        public static Indentation operator +(Indentation indent, int amount) => indent.Increase(amount);
        public static Indentation operator -(Indentation indent, int amount) => indent.Decrease(amount);

        public static Indentation operator ++(Indentation indent) => indent.Increase();
        public static Indentation operator --(Indentation indent) => indent.Decrease();
    }
}
