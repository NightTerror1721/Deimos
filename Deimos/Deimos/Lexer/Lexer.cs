#nullable enable
using Deimos.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Deimos.Lexer
{
    public sealed class Lexer
    {
        private readonly CharReader _reader;
        private readonly string _source;

        public Lexer(string source)
        {
            _reader = new CharReader(source);
            _source = source;
        }

        public IEnumerable<Token> ScanTokens()
        {
            while (true)
            {
                Token token = ScanToken();
                if (token.IsInvalid)
                    break;

                yield return token;
            }
        }

        public Token ScanToken()
        {
            if (_reader.IsAtEnd)
                return Token.Invalid(_reader.Index);

            SkipWhitespaceAndComments();

            var initialIndex = _reader.Index;
            if (_reader.IsAtEnd)
                return Token.Invalid(initialIndex);

            // Identifier or Keyword //
            if (_reader.IsCurrentLetter || _reader.Current == '_')
                return ReadIdentifierOrKeyword();

            // Number Literal //
            if (_reader.Current.IsDigit() || (_reader.Current is '+' or '-' && _reader.Peek(1).IsDigit()))
                return ReadNumber();

            // String Literal //
            if (_reader.Current is '"' or '\'')
                return ReadString();

            // Operator or Punctuation //
            var opToken = TryReadOperator();
            if (opToken is not null)
                return opToken;

            throw Error($"Unrecognized character '{_reader.Current}'", initialIndex);

        }

        private void SkipWhitespaceAndComments()
        {
            while (true)
            {
                if (_reader.MatchWhiteSpace())
                    continue;

                // Single-line comment //
                if (_reader.Match('/', '/'))
                {
                    while (!_reader.IsCurrentEndOfLineOrFile)
                        _reader.Advance();
                    continue;
                }

                // Multi-line comment //
                if (_reader.Match('/', '*'))
                {
                    while (!(_reader.Peek() == '*' && _reader.Peek(1) == '/') && !_reader.IsCurrentEndOfFile)
                        _reader.Advance();
                    _reader.Match('*', '/'); // consume '*' '/' //
                    continue;
                }

                break;
            }
        }

        private Token ReadIdentifierOrKeyword()
        {
            int initialPos = _reader.Position;
            var initialIndex = _reader.Index;

            if (_reader.IsCurrentDigit)
                throw Error("Identifier cannot start by number", initialIndex);

            while (_reader.IsCurrentLetterOrDigit || _reader.Current == '_')
                _reader.Advance();

            string lexeme = _source[initialPos.._reader.Position];

            if (TokenTypeUtils.TryGetKeyword(lexeme, out var type))
            {
                return type switch
                {
                    TokenType.True => new(TokenType.BoolLiteral, lexeme, true, initialIndex, _reader.Index),
                    TokenType.False => new(TokenType.BoolLiteral, lexeme, false, initialIndex, _reader.Index),
                    TokenType.Null => new(TokenType.NullLiteral, lexeme, null, initialIndex, _reader.Index),
                    _ => new(type, lexeme, initialIndex, _reader.Index)
                };
            }

            return new(TokenType.Identifier, lexeme, null, initialIndex, _reader.Index);
        }

        private Token ReadString()
        {
            int initialPos = _reader.Position;
            var initialIndex = _reader.Index;

            char quote = _reader.Current;
            if (quote is not '"' and not '\'')
                throw Error("String literal must start with ' or \"", initialIndex);

            // Detect block string (""" or ''') //
            bool isBlock = _reader.Peek(1) == quote && _reader.Peek(2) == quote;
            if (isBlock)
                _reader.Advance(3); // skip opening block quotes
            else
                _reader.Advance(); // skip opening quote

            var sb = new StringBuilder();

            while (!_reader.IsCurrentEndOfFile)
            {
                // Block Closing //
                if (isBlock)
                {
                    if (_reader.Current == quote && _reader.Peek(1) == quote && _reader.Peek(2) == quote)
                    {
                        _reader.Advance(3); // skip closing block quotes
                        string lexeme = _source[initialPos.._reader.Position];
                        return new Token(TokenType.StringLiteral, lexeme, sb.ToString(), initialIndex, _reader.Index);
                    }

                }
                else // Regular Closing //
                {
                    if (_reader.Current == quote)
                    {
                        _reader.Advance(); // skip closing quote
                        string lexeme = _source[initialPos.._reader.Position];
                        return new Token(TokenType.StringLiteral, lexeme, sb.ToString(), initialIndex, _reader.Index);
                    }
                }

                // Handle Escape Sequences //
                if (!isBlock && _reader.Current == '\\')
                {
                    string escaped = ParseEscapedSequence(initialIndex);
                    sb.Append(escaped);
                    continue;
                }

                sb.Append(_reader.Current);
                _reader.Advance();
            }

            throw Error("Unterminated string literal", initialIndex, _reader.Index);
        }

        private string ParseEscapedSequence(TextIndex initialIndex)
        {
            if (_reader.Current != '\\')
                Error("Expected escape character '\\'", initialIndex, _reader.Index);

            char next = _reader.Peek(1);

            // Unicode: \uXXXX or \UXXXXXXXX //
            if (next is 'u' or 'U')
            {
                _reader.Advance(2); // skip '\' 'u' or 'U'
                int count = next == 'u' ? 4 : 8;
                var hexDigits = new StringBuilder();
                for (int i = 0; i < count; i++)
                {
                    if (!_reader.Current.IsHexDigit())
                        throw Error("Invalid Unicode escape sequence", initialIndex, _reader.Index);
                    hexDigits.Append(_reader.Current);
                    _reader.Advance();
                }
                string hexString = hexDigits.ToString();
                try
                {
                    int codePoint = Convert.ToInt32(hexString, 16);
                    return char.ConvertFromUtf32(codePoint);
                }
                catch (Exception)
                {
                    throw Error("Invalid Unicode escape sequence", initialIndex, _reader.Index);
                }
            }

            _reader.Advance(); // skip '\'
            return next switch
            {
                'n' => "\n",
                'r' => "\r",
                't' => "\t",
                '\\' => "\\",
                '\'' => "'",
                '"' => "\"",
                '0' => "\0",
                _ => throw Error("Invalid escape sequence", initialIndex, _reader.Index)
            };
        }

        private Token ReadNumber()
        {
            int initialPos = _reader.Position;
            var initialIndex = _reader.Index;

            bool isNegative = false;
            if (_reader.Current is '+' or '-')
            {
                isNegative = _reader.Current == '-';
                _reader.Advance();
            }

            if (_reader.IsCurrentEndOfLineOrFile)
                throw Error("", initialIndex, _reader.Index);

            bool isBinary = false;
            bool isOctal = false;
            bool isHex = false;

            if (_reader.Current == '0')
            {
                char c = _reader.Peek(1);
                if (c is 'b' or 'B') { isBinary = true; _reader.Advance(2); }
                else if (c is 'o' or 'O') { isOctal = true; _reader.Advance(2); }
                else if (c is 'x' or 'X') { isHex = true; _reader.Advance(2); }
            }

            if (isBinary)
                return ReadBinaryOrOctalInteger(initialPos, initialIndex, isNegative, true);

            if (isOctal)
                return ReadBinaryOrOctalInteger(initialPos, initialIndex, isNegative, false);

            if (isHex)
                return ReadHexNumber(initialPos, initialIndex, isNegative);

            return ReadDecimalNumber(initialPos, initialIndex, isNegative);
        }

        private Token ReadBinaryOrOctalInteger(int initialPos, TextIndex initialIndex, bool isNegative, bool isBinary)
        {
            Func<char, bool> checkIsValidDigit = isBinary ? NumberUtils.IsBinaryDigit : NumberUtils.IsOctalDigit;
            if (!_reader.AdvanceWhile(c => checkIsValidDigit(c) || (c == '_' && checkIsValidDigit(_reader.Peek(1)))))
                throw Error("Invalid binary/octal integer literal", initialIndex, _reader.Index);

            string lexeme = _source[initialPos.._reader.Position];
            string digits = NumberUtils.ExtractDigits(lexeme, isNegative ? 3 : 2, lexeme.Length);

            try
            {
                long value = Convert.ToInt64(digits, isBinary ? 2 : 8);
                if (isNegative)
                    value = -value;
                return new Token(TokenType.IntLiteral, lexeme, value, initialIndex, _reader.Index);
            }
            catch (OverflowException)
            {
                throw Error("Integer literal is too large", initialIndex, _reader.Index);
            }
        }

        private Token ReadHexNumber(int initialPos, TextIndex initialIndex, bool isNegative)
        {
            string digits;
            bool hasBase = _reader.AdvanceWhile(c => c.IsHexDigit() || (c == '_' && _reader.Peek(1).IsHexDigit()));

            bool hasDot = false;
            if (_reader.Current == '.')
            {
                _reader.Advance(); // dot
                hasDot = _reader.AdvanceWhile(c => c.IsHexDigit() || (c == '_' && _reader.Peek(1).IsHexDigit()));
            }

            bool hasExponent = false;
            if (_reader.Current is 'p' or 'P')
            {
                _reader.Advance();

                if (_reader.Current is '+' or '-')
                    _reader.Advance();

                if (!_reader.AdvanceWhile(c => c.IsDigit() || (c == '_' && _reader.Peek(1).IsDigit())))
                    throw Error("Hexadecimal floating-point literal exponent is invalid or missing digits", initialIndex, _reader.Index);

                hasExponent = true;
            }

            string lexeme = _source[initialPos.._reader.Position];

            if (hasDot || hasExponent) // Is Decimal //
            {
                digits = NumberUtils.ExtractDigitsExceptP(lexeme, isNegative ? 3 : 2, lexeme.Length);
                try
                {
                    double doubleValue = NumberUtils.HexFloatToDouble(digits);
                    if (isNegative) doubleValue = -doubleValue;
                    return new Token(TokenType.FloatLiteral, lexeme, doubleValue, initialIndex, _reader.Index);
                }
                catch (Exception)
                {
                    throw Error("Failed to parse hexadecimal floating-point literal", initialIndex, _reader.Index);
                }
            }

            if (!hasBase)
                throw Error("Hexadecimal literal must contain at least one valid digit", initialIndex, _reader.Index);

            // Is Integer //
            digits = NumberUtils.ExtractDigits(lexeme, isNegative ? 3 : 2, lexeme.Length);
            try
            {
                long longValue = Convert.ToInt64(digits, 16);
                if (isNegative) longValue = -longValue;
                return new Token(TokenType.IntLiteral, lexeme, longValue, initialIndex, _reader.Index);
            }
            catch (Exception)
            {
                throw Error("Failed to parse hexadecimal integer literal", initialIndex, _reader.Index);
            }
        }

        private Token ReadDecimalNumber(int initialPos, TextIndex initialIndex, bool isNegative)
        {
            bool hasDigits = false;
            bool isInteger = true;

            // Integral part //
            if (_reader.Current.IsDigit())
                hasDigits = _reader.AdvanceWhile(c => c.IsDigit() || (c == '_' && _reader.Peek(1).IsDigit()));

            // Decimal part //
            if (_reader.Current == '.')
            {
                isInteger = false;
                _reader.Advance();
                if (_reader.Current.IsDigit() || _reader.Current == '_')
                    hasDigits |= _reader.AdvanceWhile(c => c.IsDigit() || (c == '_' && _reader.Peek(1).IsDigit()));
            }

            if (!hasDigits)
                throw Error("Decimal number literal must contain at least one digit", initialIndex, _reader.Index);

            // Exponent part //
            if (_reader.Current is 'e' or 'E')
            {
                isInteger = false;
                _reader.Advance();

                if (_reader.Current is '+' or '-')
                    _reader.Advance();

                if (!_reader.AdvanceWhile(c => c.IsDigit() || (c == '_' && _reader.Peek(1).IsDigit())))
                    throw Error("Exponent part of decimal number literal is invalid or missing digits", initialIndex, _reader.Index);
            }

            string lexeme = _source[initialPos.._reader.Position];
            string digits = NumberUtils.ExtractDigits(lexeme, isNegative ? 3 : 2, lexeme.Length);

            if (isInteger)
            {
                if (!long.TryParse(digits, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
                    throw Error("Failed to parse integer decimal literal", initialIndex, _reader.Index);

                if (isNegative) longValue = -longValue;
                return new Token(TokenType.IntLiteral, lexeme, longValue, initialIndex, _reader.Index);
            }

            if (!double.TryParse(digits, NumberStyles.Float, CultureInfo.InvariantCulture, out var doubleValue))
                throw Error("Failed to parse floating-point decimal literal", initialIndex, _reader.Index);

            if (isNegative) doubleValue = -doubleValue;
            return new Token(TokenType.FloatLiteral, lexeme, doubleValue, initialIndex, _reader.Index);
        }

        private Token? TryReadOperator()
        {
            var initialIndex = _reader.Index;
            char c = _reader.Advance();

            switch (c)
            {
                case '(': return new Token(TokenType.LeftParen, "(", initialIndex, _reader.Index);
                case ')': return new Token(TokenType.RightParen, ")", initialIndex, _reader.Index);
                case '{': return new Token(TokenType.LeftBrace, "{", initialIndex, _reader.Index);
                case '}': return new Token(TokenType.RightBrace, "{", initialIndex, _reader.Index);
                case '[': return new Token(TokenType.LeftBracket, "[", initialIndex, _reader.Index);
                case ']': return new Token(TokenType.RightBracket, "]", initialIndex, _reader.Index);
                case ',': return new Token(TokenType.Comma, ",", initialIndex, _reader.Index);
                case ';': return new Token(TokenType.Semicolon, ";", initialIndex, _reader.Index);
                case ':': return new Token(TokenType.Colon, ":", initialIndex, _reader.Index);
                case '.':
                    if (_reader.Match('.', '.')) return new Token(TokenType.Elipsis, "...", initialIndex, _reader.Index);
                    if (_reader.Match('.')) return new Token(TokenType.DotDot, "..", initialIndex, _reader.Index);
                    return new Token(TokenType.Dot, ".", initialIndex, _reader.Index);
                case '+':
                    if (_reader.Match('+')) return new Token(TokenType.PlusPlus, "++", initialIndex, _reader.Index);
                    if (_reader.Match('=')) return new Token(TokenType.PlusEqual, "+=", initialIndex, _reader.Index);
                    return new Token(TokenType.Plus, "+", initialIndex, _reader.Index);
                case '-':
                    if (_reader.Match("-")) return new Token(TokenType.MinusMinus, "--", initialIndex, _reader.Index);
                    if (_reader.Match('=')) return new Token(TokenType.MinusEqual, "-=", initialIndex, _reader.Index);
                    if (_reader.Match('>')) return new Token(TokenType.Arrow, "->", initialIndex, _reader.Index);
                    return new Token(TokenType.Plus, "-", initialIndex, _reader.Index);
                case '*':
                    if (_reader.Match("*"))
                    {
                        if (_reader.Match('=')) return new Token(TokenType.AsteriskAsteriskEqual, "**=", initialIndex, _reader.Index);
                        return new Token(TokenType.AsteriskAsterisk, "**", initialIndex, _reader.Index);
                    }
                    if (_reader.Match('=')) return new Token(TokenType.AsteriskEqual, "*=", initialIndex, _reader.Index);
                    return new Token(TokenType.Asterisk, "*", initialIndex, _reader.Index);
                case '/':
                    if (_reader.Match('=')) return new Token(TokenType.SlashEqual, "/=", initialIndex, _reader.Index);
                    return new Token(TokenType.Slash, "/", initialIndex, _reader.Index);
                case '%':
                    if (_reader.Match('=')) return new Token(TokenType.PercentEqual, "%=", initialIndex, _reader.Index);
                    return new Token(TokenType.Percent, "%", initialIndex, _reader.Index);
                case '?':
                    if (_reader.Match('?'))
                    {
                        if (_reader.Match('=')) return new Token(TokenType.NullCoalesceEqual, "??=", initialIndex, _reader.Index);
                        return new Token(TokenType.NullCoalesce, "??", initialIndex, _reader.Index);
                    }
                    return new Token(TokenType.Question, "?", initialIndex, _reader.Index);
                case '!':
                    if (_reader.Match('=')) return new Token(TokenType.BangEqual, "!=", initialIndex, _reader.Index);
                    return new Token(TokenType.Bang, "!", initialIndex, _reader.Index);
                case '=':
                    if (_reader.Match('=')) return new Token(TokenType.EqualEqual, "==", initialIndex, _reader.Index);
                    return new Token(TokenType.Equal, "=", initialIndex, _reader.Index);
                case '>':
                    if (_reader.Match('>'))
                    {
                        if (_reader.Match('=')) return new Token(TokenType.ShiftRightEqual, ">>=", initialIndex, _reader.Index);
                        return new Token(TokenType.ShiftRight, ">>", initialIndex, _reader.Index);
                    }
                    if (_reader.Match('=')) return new Token(TokenType.GreaterEqual, ">=", initialIndex, _reader.Index);
                    return new Token(TokenType.Greater, ">", initialIndex, _reader.Index);
                case '<':
                    if (_reader.Match('<'))
                    {
                        if (_reader.Match('=')) return new Token(TokenType.ShiftLeftEqual, "<<=", initialIndex, _reader.Index);
                        return new Token(TokenType.ShiftLeft, "<<", initialIndex, _reader.Index);
                    }
                    if (_reader.Match('=')) return new Token(TokenType.LessEqual, "<=", initialIndex, _reader.Index);
                    return new Token(TokenType.Less, "<", initialIndex, _reader.Index);
                case '&':
                    if (_reader.Match('&')) return new Token(TokenType.AmpersandAmpersand, "&&", initialIndex, _reader.Index);
                    if (_reader.Match('=')) return new Token(TokenType.AmpersandEqual, "&=", initialIndex, _reader.Index);
                    return new Token(TokenType.Ampersand, "&", initialIndex, _reader.Index);
                case '|':
                    if (_reader.Match('|')) return new Token(TokenType.PipePipe, "||", initialIndex, _reader.Index);
                    if (_reader.Match('=')) return new Token(TokenType.PipeEqual, "|=", initialIndex, _reader.Index);
                    return new Token(TokenType.Pipe, "|", initialIndex, _reader.Index);
                case '^':
                    if (_reader.Match('=')) return new Token(TokenType.CaretEqual, "^=", initialIndex, _reader.Index);
                    return new Token(TokenType.Caret, "^", initialIndex, _reader.Index);
                case '~': return new Token(TokenType.Tilde, "~", initialIndex, _reader.Index);
            }

            return null;
        }

        private DeimosLexerException Error(string message, TextIndex from, TextIndex? to = null)
        {
            TextRange range = to.HasValue ? TextRange.From(from, to.Value) : TextRange.From(from, from);
            return new DeimosLexerException(message, Token.Invalid(range));
        }
    }
}
