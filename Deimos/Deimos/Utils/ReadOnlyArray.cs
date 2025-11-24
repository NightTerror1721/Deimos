#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Deimos.Utils
{
    public readonly struct ReadOnlyArray<T> : IReadOnlyList<T>, IEquatable<ReadOnlyArray<T>>
    {
        private readonly T[] _array;

        public ReadOnlyArray(T[] array)
        {
            if (array == null)
                _array = Array.Empty<T>();
            else
                _array = (T[])array.Clone();
        }

        public ReadOnlyArray(ReadOnlyArray<T> readOnlyArray) => _array = readOnlyArray._array;
        public ReadOnlyArray(ReadOnlySpan<T> span) => _array = span.ToArray();
        public ReadOnlyArray(Span<T> span) => _array = span.ToArray();
        public ReadOnlyArray(IEnumerable<T> collection) => _array = collection is T[] arr ? (T[])arr.Clone() : (collection ?? Array.Empty<T>()).ToArray();

        public static explicit operator T[](ReadOnlyArray<T> readOnlyArray) => (T[])readOnlyArray._array.Clone();
        public static implicit operator ReadOnlyArray<T>(T[] array) => new(array);

        public static explicit operator ReadOnlyArray<T>(ReadOnlySpan<T> span) => new(span);
        public static explicit operator ReadOnlyArray<T>(Span<T> span) => new(span);

        public int Length => _array.Length;
        public int Count => _array.Length;

        public T this[int index] => _array[index];

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_array).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _array.GetEnumerator();

        public ReadOnlySpan<T> AsSpan() => _array.AsSpan();

        public bool SequenceEqual(ReadOnlyArray<T> other) => _array.SequenceEqual(other._array);

        public bool Equals(ReadOnlyArray<T> other) => ReferenceEquals(_array, other._array);
        public bool Equals(T[]? otherArray) => ReferenceEquals(_array, otherArray);

        public override bool Equals(object? obj) => obj is ReadOnlyArray<T> other && Equals(other);

        public override int GetHashCode() => _array.GetHashCode();

        public override string ToString() => _array.ToString();

        public static bool operator ==(ReadOnlyArray<T> left, ReadOnlyArray<T> right) => left.Equals(right);
        public static bool operator !=(ReadOnlyArray<T> left, ReadOnlyArray<T> right) => !left.Equals(right);
    }

    public static class ReadOnlyArray
    {
        public static ReadOnlyArray<T> Create<T>(params T[] items) => new(items);

        public static ReadOnlyArray<T> FromArray<T>(T[] array) => new(array);
        public static ReadOnlyArray<T> FromSpan<T>(ReadOnlySpan<T> span) => new(span);
        public static ReadOnlyArray<T> FromSpan<T>(Span<T> span) => new(span);
        public static ReadOnlyArray<T> FromEnumerable<T>(IEnumerable<T> collection) => new(collection);

        public static ReadOnlyArray<T> Empty<T>() => new(Array.Empty<T>());

        public static ReadOnlyArray<T> ToReadOnlyArray<T>(this T[] array) => new(array);
        public static ReadOnlyArray<T> ToReadOnlyArray<T>(this IEnumerable<T> enumerable) => new(enumerable);
    }
}
