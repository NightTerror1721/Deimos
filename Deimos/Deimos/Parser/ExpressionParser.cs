#nullable enable

using Deimos.AST;
using Deimos.AST.Expressions;
using Deimos.Exceptions;
using Deimos.Lexer;
using System.Collections.Generic;

namespace Deimos.Parser
{
    public sealed partial class Parser
    {
        public Expression ParseExpression() => ParseExpression(1);

        private Expression ParseExpression(int minPrecedence)
        {
            Expression left = ParseUnaryAndPostfixExpression();

            while (!_reader.IsAtEnd)
            {
                Token opToken = _reader.Current;
                if (!opToken.TryToBinaryOperator(out var op))
                    break;

                int prec = op.GetPrecedence();
                if (prec < minPrecedence)
                    break;

                _reader.Advance();

                int nextMinPrec = op.IsRightAssociative() ? prec : prec + 1;

                // Special handling for 'as' and 'instanceof' which are not typical binary operators
                if (op == BinaryOperator.As)
                {
                    TypeNode targetType = ParseType();
                    left = new CastExpression(left, targetType, left.From, targetType.To);
                    continue;
                }
                if (op == BinaryOperator.InstanceOf)
                {
                    TypeNode checkType = ParseType();
                    left = new InstanceOfExpression(left, checkType, left.From, checkType.To);
                    continue;
                }

                Expression right = ParseExpression(nextMinPrec);
                left = new BinaryExpression(left, op, right, left.From, right.To);
            }

            return left;
        }

        private Expression ParsePrefixUnaryExpression()
        {
            var initialIndex = _reader.CurrentStartIndex;
            Token token = _reader.Current;

            UnaryOperator? op = token.Type switch
            {
                TokenType.Plus => UnaryOperator.Positivation,
                TokenType.Minus => UnaryOperator.Negation,
                TokenType.Bang => UnaryOperator.LogicalNot,
                TokenType.Tilde => UnaryOperator.BinaryNot,
                TokenType.PlusPlus => UnaryOperator.PreIncrement,
                TokenType.MinusMinus => UnaryOperator.PreDecrement,
                _ => null
            };

            if (op is not null)
            {
                _reader.Advance();
                Expression right = ParsePrefixUnaryExpression();
                return new UnaryExpression(op.Value, right, initialIndex, right.To);
            }

            return ParsePrimaryExpression();
        }

        private Expression ParseUnaryAndPostfixExpression()
        {
            var expr = ParsePrefixUnaryExpression();

            while (true)
            {
                Token token = _reader.Current;
                switch (token.Type)
                {
                    case TokenType.Bang:
                    {
                        _reader.Advance();
                        expr = new UnaryExpression(UnaryOperator.NonNullAssertion, expr, expr.From, token.To);
                        continue;
                    }
                    case TokenType.PlusPlus:
                    {
                        _reader.Advance();
                        expr = new UnaryExpression(UnaryOperator.PostIncrement, expr, expr.From, token.To);
                        continue;
                    }
                    case TokenType.MinusMinus:
                    {
                        _reader.Advance();
                        expr = new UnaryExpression(UnaryOperator.PostDecrement, expr, expr.From, token.To);
                        continue;
                    }
                }

                // Other postfix: calls, (safe)member access, index access

                // Call
                if (_reader.Match(TokenType.LeftParen))
                {
                    expr = ParseCallExpression(expr, token);
                    continue;
                }

                // Member Access
                if (_reader.Match(TokenType.Dot))
                {
                    Token name = _reader.Consume(TokenType.Identifier, "Expected identifier after '.'");
                    expr = new MemberAccessExpression(expr, name.Lexeme, isSafeAccess: false, expr.From, name.To);
                    continue;
                }

                // Null-Safe Member Access
                if (_reader.Match(TokenType.NullSafeAccess))
                {
                    Token name = _reader.Consume(TokenType.Identifier, "Expected identifier after '?.'");
                    expr = new MemberAccessExpression(expr, name.Lexeme, isSafeAccess: true, expr.From, name.To);
                    continue;
                }

                // Index Access
                if (_reader.Match(TokenType.LeftBracket))
                {
                    Token open = token;
                    Expression index = ParseExpression();
                    _reader.Expect(TokenType.RightBracket, "Expected ']' after index expression");
                    expr = new IndexExpression(expr, index, open.From, index.To);
                    continue;
                }

                break;
            }

            return expr;
        }

        private Expression ParseCallExpression(Expression callee, Token openParen)
        {
            var args = new List<Expression>();
            if (!_reader.Check(TokenType.RightParen))
            {
                do
                {
                    args.Add(ParseExpression());
                } while (_reader.Match(TokenType.Comma));
            }

            _reader.Expect(TokenType.RightParen, "Expected ')' after arguments");
            return new CallExpression(callee, args, openParen.From, _reader.Previous.To);
        }

        private Expression ParseNewExpression(Token newToken)
        {
            TypeNode type = ParseType();

            // new Array[size]
            if (_reader.Match(TokenType.LeftBracket))
            {
                Expression size = ParseExpression();
                _reader.Expect(TokenType.RightBracket, "Expected ']' after new array expression");
                return new NewArrayExpression(type, size, newToken.From, _reader.Previous.To);
            }

            // new Type(args)
            if (_reader.Match(TokenType.LeftParen))
            {
                var args = new List<Expression>();
                if (!_reader.Check(TokenType.RightParen))
                {
                    do
                    {
                        args.Add(ParseExpression());
                    } while (_reader.Match(TokenType.Comma));
                }
                _reader.Expect(TokenType.RightParen, "Expected ')' after constructor arguments");
                return new NewObjectExpression(type, args, newToken.From, _reader.Previous.To);
            }

            throw new DeimosParserException("Expected '[' for array or '(' for object instantiation after 'new'", newToken.From, newToken.To);
        }

        private Expression ParsePrimaryExpression()
        {
            Token token = _reader.Current;

            // Literals
            if (_reader.Match(TokenType.IntLiteral,
                              TokenType.FloatLiteral,
                              TokenType.StringLiteral,
                              TokenType.BoolLiteral,
                              TokenType.NullLiteral))
            {
                return new LiteralExpression(token.Value, token.From, token.To);
            }

            // Identifier
            if (_reader.Match(TokenType.Identifier))
                return new IdentifierExpression(token.Lexeme, token.From, token.To);

            // New Expression
            if (_reader.Match(TokenType.New))
                return ParseNewExpression(token);

            // Array Literal
            if (_reader.Match(TokenType.LeftBracket))
            {
                Token open = token;
                var elements = new List<Expression>();

                if (!_reader.Check(TokenType.RightBracket))
                {
                    do
                    {
                        elements.Add(ParseExpression());
                    } while (_reader.Match(TokenType.Comma));
                }

                _reader.Expect(TokenType.RightBracket, "Expected ']' after array literal elements");
                return new ArrayLiteralExpression(elements, open.From, _reader.Previous.To);
            }

            // Dictionary Literal
            if (_reader.Match(TokenType.LeftBrace))
            {
                Token open = token;
                var entries = new List<(Expression, Expression)>();

                if (!_reader.Check(TokenType.RightBrace))
                {
                    do
                    {
                        Expression key = ParseExpression();
                        _reader.Expect(TokenType.Colon, "Expected ':' after dictionary key");
                        Expression value = ParseExpression();
                        entries.Add((key, value));
                    } while (_reader.Match(TokenType.Comma));
                }

                _reader.Expect(TokenType.RightBrace, "Expected '}' after dictionary literal entries");
                return new DictionaryLiteralExpression(entries, open.From, _reader.Previous.To);
            }

            // Parenthesis: (expr) or literal tuple
            if (_reader.Match(TokenType.LeftParen))
            {
                Token open = token;

                // Check for empty tuple
                if (_reader.Match(TokenType.Comma))
                {
                    _reader.Expect(TokenType.RightParen, "Expected ')' after ',' in empty tuple");
                    return new TupleLiteralExpression(System.Array.Empty<Expression>(), open.From, _reader.Previous.To);
                }

                Expression first = ParseExpression();
                if (_reader.Match(TokenType.RightParen))
                    return new GroupExpression(first, open.From, _reader.Previous.To);

                _reader.Expect(TokenType.Comma, "Expected ',' or ')' in tuple literal");

                var elements = new List<Expression> { first };
                do
                {
                    if (_reader.Current.Type == TokenType.RightBrace)
                        break;
                    elements.Add(ParseExpression());
                } while (_reader.Match(TokenType.Comma));

                _reader.Expect(TokenType.RightParen, "Expected ')' after tuple literal elements");

                return new TupleLiteralExpression(elements, open.From, _reader.Previous.To);
            }

            throw new DeimosParserException($"Unexpected token '{token.Lexeme}' in expression", token.From, token.To);
        }
    }
}
