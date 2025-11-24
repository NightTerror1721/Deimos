#nullable enable

namespace Deimos.Lexer
{
    public sealed class Token
    {
        public TokenType Type { get; }
        public string Lexeme { get; }
        public object? Value { get; }
        public TextRange Range { get; }

        public TextIndex From => Range.Start;
        public TextIndex To => Range.End;

        public int FromLine => Range.Start.Line;
        public int FromColumn => Range.Start.Column;
        public int ToLine => Range.End.Line;
        public int ToColumn => Range.End.Column;

        public bool IsValid => Type != TokenType.Invalid;
        public bool IsInvalid => Type == TokenType.Invalid;

        public Token(TokenType type, string lexeme, object? value, TextRange range)
        {
            Type = type;
            Lexeme = lexeme;
            Value = value;
            Range = range;
        }
        public Token(TokenType type, string lexeme, object? value, TextIndex from, TextIndex to) : this(type, lexeme, value, TextRange.From(from, to)) { }
        public Token(TokenType type, string lexeme, object? value, TextIndex index) : this(type, lexeme, value, TextRange.From(index, index)) { }
        public Token(TokenType type, string lexeme, object? value, int fromLine, int fromColumn, int toLine, int toColumn) :
            this(type, lexeme, value, TextRange.From(fromLine, fromColumn, toLine, toColumn)) { }
        public Token(TokenType type, string lexeme, object? value, int line, int column) :
            this(type, lexeme, value, TextRange.From(line, column, line, column))
        { }

        public static Token Invalid(TextRange range) => new(TokenType.Invalid, string.Empty, null, range);
        public static Token Invalid(TextIndex from, TextIndex to) => new(TokenType.Invalid, string.Empty, null, from, to);
        public static Token Invalid(TextIndex index) => new(TokenType.Invalid, string.Empty, null, index);
        public static Token Invalid(int fromLine, int fromColumn, int toLine, int toColumn) =>
            new(TokenType.Invalid, string.Empty, null, fromLine, fromColumn, toLine, toColumn);
        public static Token Invalid(int line, int column) => new(TokenType.Invalid, string.Empty, null, line, column);

        public override string ToString()
        {
            return $"{Type} '{Lexeme}' ({Range})";
        }
    }
}
