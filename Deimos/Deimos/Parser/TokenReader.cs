#nullable enable

using Deimos.Exceptions;
using Deimos.Lexer;
using System.Collections.Generic;
using System.Diagnostics;

namespace Deimos.Parser
{
    public sealed class TokenReader
    {
        private readonly IReadOnlyList<Token> _tokens;
        private readonly Token _defaultInvalidToken;
        private int _position;

        public TokenReader(IReadOnlyList<Token> tokens, int initialPosition = 0)
        {
            if (initialPosition < 0 || initialPosition > tokens.Count)
                throw new System.ArgumentOutOfRangeException(nameof(initialPosition), "Initial position is out of range of the token list.");

            _tokens = tokens ?? throw new System.ArgumentNullException(nameof(tokens));
            _defaultInvalidToken = tokens.Count > 0 ? Token.Invalid(tokens[^1].Range) : Token.Invalid(TextRange.Zero);
            _position = initialPosition;
        }
        private TokenReader(TokenReader other, int offset)
        {
            _tokens = other._tokens;
            _defaultInvalidToken = other._defaultInvalidToken;
            _position = other._position + offset;

            if (_position < 0 || _position > _tokens.Count)
                throw new System.ArgumentOutOfRangeException(nameof(offset), "Resulting position is out of range of the token list.");
        }

        public IReadOnlyList<Token> Tokens => _tokens;
        public int Position
        {
            get => _position;
            set => SetPosition(value);
        }

        public int Count => _tokens.Count;

        public Token Current => Peek(0);
        public Token Next => Peek(1);
        public Token Previous => Peek(-1);

        public bool IsAtEnd => _position >= _tokens.Count;
        public bool IsCurrentInvalid => Current.IsInvalid;

        public TextIndex CurrentStartIndex => Current.From;
        public TextIndex CurrentEndIndex => Current.To;


        public TokenReader Copy(int offset = 0) => new(this, offset);

        public Token Peek(int offset = 0)
        {
            int index = _position + offset;
            if (index < 0 || index >= _tokens.Count)
                return _defaultInvalidToken;
            return _tokens[index];
        }

        public Token Advance()
        {
            if (_position >= _tokens.Count)
                return _defaultInvalidToken;
            return _tokens[++_position];
        }
        public Token Advance(int amount)
        {
            if (amount < 0)
                return Retreat(-amount);

            int newPosition = _position + amount;
            if (newPosition > _tokens.Count)
            {
                _position = _tokens.Count;
                return _defaultInvalidToken;
            }

            _position = newPosition;
            return _tokens[_position];
        }

        public Token Retreat()
        {
            if (_position <= 0)
                return _defaultInvalidToken;
            return _tokens[--_position];
        }
        public Token Retreat(int amount)
        {
            if (amount < 0)
                return Advance(-amount);

            int newPosition = _position - amount;
            if (newPosition < 0)
            {
                _position = 0;
                return _defaultInvalidToken;
            }

            _position = newPosition;
            return _tokens[_position];
        }

        public Token SetPosition(int position)
        {
            if (position == _position)
                return _tokens[_position];

            if (position < 0)
            {
                _position = 0;
                return _defaultInvalidToken;
            }
            if (position >= _tokens.Count)
            {
                _position = _tokens.Count;
                return _defaultInvalidToken;
            }

            _position = position;
            return _tokens[_position];
        }

        public bool Match(TokenType type)
        {
            if (IsAtEnd || _tokens[_position].Type != type)
                return false;

            Advance();
            return true;
        }

        public bool Match(TokenType type1, TokenType type2)
        {
            if (_position + 1 >= _tokens.Count || (_tokens[_position].Type != type1 && _tokens[_position + 1].Type != type2))
                return false;

            Advance(2);
            return true;
        }

        public bool Match(TokenType type1, TokenType type2, TokenType type3)
        {
            if (_position + 2 >= _tokens.Count || (_tokens[_position].Type != type1 && _tokens[_position + 1].Type != type2 && _tokens[_position + 2].Type != type3))
                return false;

            Advance(3);
            return true;
        }

        public bool Match(params TokenType[] tokens)
        {
            if (_position + tokens.Length >= _tokens.Count)
                return false;

            for (int i = 0; i < tokens.Length; i++)
            {
                if (_tokens[_position + i].Type != tokens[i])
                    return false;
            }

            Advance(tokens.Length);
            return false;
        }

        public bool Check(TokenType type)
        {
            if (IsAtEnd)
                return false;
            return _tokens[_position].Type == type;
        }

        public bool Check(TokenType type1, TokenType type2)
        {
            if (_position + 1 >= _tokens.Count)
                return false;
            return _tokens[_position].Type == type1 && _tokens[_position + 1].Type == type2;
        }

        public bool Check(TokenType type1, TokenType type2, TokenType type3)
        {
            if (_position + 2 >= _tokens.Count)
                return false;
            return _tokens[_position].Type == type1 && _tokens[_position + 1].Type == type2 && _tokens[_position + 2].Type == type3;
        }

        public bool Check(params TokenType[] types)
        {
            if (_position + types.Length - 1 >= _tokens.Count)
                return false;

            for (int i = 0; i < types.Length; i++)
            {
                if (_tokens[_position + i].Type != types[i])
                    return false;
            }
            return true;
        }

        [DebuggerStepThrough]
        public void Expect(TokenType type, string message)
        {
            if (IsAtEnd)
                throw new DeimosParserException(message, _defaultInvalidToken);

            if (_tokens[_position].Type != type)
                throw new DeimosParserException(message, _tokens[_position].From, _tokens[_position].To);

            Advance();
        }

        [DebuggerStepThrough]
        public void Expect(TokenType type1, TokenType type2, string message)
        {
            if (_position + 1 >= _tokens.Count)
            {
                if (IsAtEnd)
                    throw new DeimosParserException(message, _defaultInvalidToken);
                throw new DeimosParserException(message, _tokens[_position].From, _defaultInvalidToken.To);
            }

            if (_tokens[_position].Type != type1 || _tokens[_position + 1].Type != type2)
                throw new DeimosParserException(message, _tokens[_position].From, _tokens[_position + 1].To);

            Advance(2);
        }

        [DebuggerStepThrough]
        public void Expect(TokenType type1, TokenType type2, TokenType type3, string message)
        {
            if (_position + 2 >= _tokens.Count)
            {
                if (IsAtEnd)
                    throw new DeimosParserException(message, _defaultInvalidToken);
                throw new DeimosParserException(message, _tokens[_position].From, _defaultInvalidToken.To);
            }

            if (_tokens[_position].Type != type1 || _tokens[_position + 1].Type != type2 || _tokens[_position + 2].Type != type3)
                throw new DeimosParserException(message, _tokens[_position].From, _tokens[_position + 2].To);

            Advance(3);
        }

        [DebuggerStepThrough]
        public void Expect(string message, params TokenType[] types)
        {
            if (types.Length == 0)
                return;

            if (_position + types.Length - 1 >= _tokens.Count)
            {
                if (IsAtEnd)
                    throw new DeimosParserException(message, _defaultInvalidToken);
                throw new DeimosParserException(message, _tokens[_position].From, _defaultInvalidToken.To);
            }

            for (int i = 0; i < types.Length; i++)
            {
                if (_tokens[_position + i].Type != types[i])
                    throw new DeimosParserException(message, _tokens[_position].From, _tokens[_position + types.Length - 1].To);
            }

            Advance(types.Length);
        }

        public Token Consume(TokenType type, string message)
        {
            Expect(type, message);
            return _tokens[_position - 1];
        }

        public (Token t1, Token t2) Consume(TokenType type1, TokenType type2, string message)
        {
            Expect(type1, type2, message);
            return (_tokens[_position - 2], _tokens[_position - 1]);
        }

        public (Token t1, Token t2, Token t3) Consume(TokenType type1, TokenType type2, TokenType type3, string message)
        {
            Expect(type1, type2, type3, message);
            return (_tokens[_position - 3], _tokens[_position - 2], _tokens[_position - 1]);
        }

        public Token[] Consume(string message, params TokenType[] types)
        {
            Expect(message, types);
            Token[] consumedTokens = new Token[types.Length];
            for (int i = 0; i < types.Length; i++)
                consumedTokens[i] = _tokens[_position - types.Length + i];
            return consumedTokens;
        }
    }
}
