#nullable enable

using System;

namespace Deimos.Lexer
{
    public sealed class CharReader
    {
        public const char EndOfFile = '\0';
        public const char EndOfLine = '\n';

        private readonly string _source;
        private int _position = 0;
        private int _line = 1;
        private int _column = 1;

        public string Source => _source;
        public TextIndex Index => TextIndex.Of(_line, _column);
        public int Position => _position;
        public int Line => _line;
        public int Column => _column;

        public char Current => Peek(0);

        public bool IsAtEnd => _position >= _source.Length;

        public bool IsCurrentWhiteSpace => char.IsWhiteSpace(Current);
        public bool IsCurrentEndOfLine => Current == EndOfLine;
        public bool IsCurrentEndOfFile => Current == EndOfFile;
        public bool IsCurrentEndOfLineOrFile
        {
            get
            {
                char current = Current;
                return current == EndOfLine || current == EndOfFile;
            }
        }

        public bool IsCurrentDigit => char.IsDigit(Current);
        public bool IsCurrentLetter => char.IsLetter(Current);
        public bool IsCurrentLetterOrDigit => char.IsLetterOrDigit(Current);

        public CharReader(string source)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
        }

        public char Peek(int lookahead = 0)
        {
            int idx = _position + lookahead;
            return idx >= _source.Length ? EndOfFile : _source[idx];
        }

        public string Substring(int start, int length)
        {
            if (start < 0 || length <= 0 || start + length > _position)
                throw new ArgumentOutOfRangeException("Start and length must define a valid range within the source string.");
            return _source.Substring(start, length);
        }

        public string Substring(int start, bool includeCurrentPosition = false)
        {
            int end = includeCurrentPosition ? _position : _position - 1;
            if (start < 0 || start > end)
                throw new ArgumentOutOfRangeException("Start must define a valid range within the source string.");
            return _source.Substring(start, end - start);
        }

        public char Advance()
        {
            char current = Peek();
            _position++;

            if (current == EndOfLine)
            {
                _line++;
                _column = 1;
            }
            else
                _column++;

            return current;
        }

        public char Advance(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");

            char lastChar = EndOfFile;
            for (int i = 0; i < count; i++)
                lastChar = Advance();
            return lastChar;
        }

        public char Retreat()
        {
            if (_position == 0)
                throw new InvalidOperationException("Cannot retreat before the start of the source.");

            _position--;
            char current = Peek();
            if (current == EndOfLine)
            {
                _line--;
                // Recalculate column by scanning backwards to the previous newline
                _column = 1;
                for (int i = _position - 1; i >= 0; i--)
                {
                    if (_source[i] == EndOfLine)
                        break;
                    _column++;
                }
            }
            else
                _column--;
            return current;
        }

        public char Retreat(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");

            char lastChar = EndOfFile;
            for (int i = 0; i < count; i++)
                lastChar = Retreat();
            return lastChar;
        }

        public bool Match(char expected)
        {
            if (Peek() != expected)
                return false;

            Advance();
            return true;
        }

        public bool Match(char expected1, char expected2)
        {
            if (Peek() != expected1 || Peek(1) != expected2)
                return false;

            Advance(2);
            return true;
        }

        public bool Match(char expected1, char expected2, char expected3)
        {
            if (Peek() != expected1 || Peek(1) != expected2 || Peek(2) != expected3)
                return false;

            Advance(3);
            return true;
        }

        public bool Match(params char[] expected)
        {
            if (expected.Length == 0)
                throw new ArgumentException("Expected array must contain at least one character.", nameof(expected));

            for (int i = 0; i < expected.Length; i++)
                if (Peek(i) != expected[i])
                    return false;

            Advance(expected.Length);
            return true;
        }

        public bool Match(string expected)
        {
            if (string.IsNullOrEmpty(expected))
                throw new ArgumentException("Expected string must not be null or empty.", nameof(expected));

            for (int i = 0; i < expected.Length; i++)
                if (Peek(i) != expected[i])
                    return false;

            Advance(expected.Length);
            return true;
        }

        public bool MatchWhiteSpace()
        {
            if (!IsCurrentWhiteSpace)
                return false;

            Advance();
            return true;
        }

        public bool MatchEndOfLine()
        {
            if (!IsCurrentEndOfLine)
                return false;

            Advance();
            return true;
        }

        public bool AdvanceWhile(Func<char, bool> predicate)
        {
            bool advanced = false;
            while (predicate(Current) && !IsCurrentEndOfFile)
            {
                Advance();
                advanced = true;
            }
            return advanced;
        }

        public bool AdvanceWhile(Func<int, char, bool> predicate)
        {
            bool advanced = false;
            while (predicate(_position, Current) && !IsCurrentEndOfFile)
            {
                Advance();
                advanced = true;
            }
            return advanced;
        }
    }
}
