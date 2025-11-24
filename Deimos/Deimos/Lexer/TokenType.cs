#nullable enable

using System.Collections.Generic;

namespace Deimos.Lexer
{
    public enum TokenType
    {
        // Invalid token //
        Invalid = 0,        // represents an invalid or unrecognized token

        // Single-character tokens //
        LeftParen,          // (
        RightParen,         // )
        LeftBrace,          // {
        RightBrace,         // }
        LeftBracket,        // [
        RightBracket,       // ]
        Comma,              // ,
        Dot,                // .
        Semicolon,          // ;
        Colon,              // :
        Plus,               // +
        Minus,              // -
        Asterisk,           // *
        Slash,              // /
        Percent,            // %
        Question,           // ?
        Bang,               // !
        Equal,              // =
        Greater,            // >
        Less,               // <
        Ampersand,          // &
        Pipe,               // |
        Caret,              // ^
        Tilde,              // ~

        // Two-character tokens //
        PlusPlus,           // ++
        MinusMinus,         // --
        ShiftLeft,          // <<
        ShiftRight,         // >>
        StarStar,           // **
        PlusEqual,          // +=
        MinusEqual,         // -=
        StarEqual,          // *=
        SlashEqual,         // /=
        PercentEqual,       // %=
        AmpersandEqual,     // &=
        PipeEqual,          // |=
        CaretEqual,         // ^=
        EqualEqual,         // ==
        BangEqual,          // !=
        GreaterEqual,       // >=
        LessEqual,          // <=
        AmpersandAmpersand, // &&
        PipePipe,           // ||
        NullSafeAccess,     // ?.
        NullCoalesce,       // ??
        Arrow,              // ->
        DotDot,             // ..

        // Three-character tokens //
        ShiftLeftEqual,     // <<=
        ShiftRightEqual,    // >>=
        StarStarEqual,      // **=
        NullCoalesceEqual,  // ??=
        Elipsis,            // ...

        // Literals //
        Identifier,         // variable names, function names, etc.
        IntLiteral,         // 123, 0b1111011, 0o173, 0x7B
        FloatLiteral,       // 123.45, 1.23e4
        StringLiteral,      // "hello", 'c'
        BoolLiteral,        // true, false
        NullLiteral,        // null

        // Keywords //
        Func,               // func: function declaration
        Class,              // class: class declaration
        Var,                // var: variable declaration
        Const,              // const: constant declaration
        If,                 // if
        Else,               // else
        While,              // while
        For,                // for
        Foreach,            // foreach
        Return,             // return
        True,               // true
        False,              // false
        Null,               // null

        // Primitive Types //
        Int,                // int
        Float,              // float
        Bool,               // bool
        String,             // string
        Void,               // void
        Any,                // any
    }

    public static class TokenTypeUtils
    {
        private static readonly Dictionary<string, TokenType> keywords = new()
        {
            { "func", TokenType.Func },
            { "class", TokenType.Class },
            { "var", TokenType.Var },
            { "const", TokenType.Const },
            { "if", TokenType.If },
            { "else", TokenType.Else },
            { "while", TokenType.While },
            { "for", TokenType.For },
            { "foreach", TokenType.Foreach },
            { "return", TokenType.Return },
            { "true", TokenType.True },
            { "false", TokenType.False },
            { "null", TokenType.Null },
            { "int", TokenType.Int },
            { "float", TokenType.Float },
            { "bool", TokenType.Bool },
            { "string", TokenType.String },
            { "void", TokenType.Void },
            { "any", TokenType.Any }
        };

        public static bool TryGetKeyword(string lexeme, out TokenType type)
        {
            return keywords.TryGetValue(lexeme, out type);
        }
    }
}
