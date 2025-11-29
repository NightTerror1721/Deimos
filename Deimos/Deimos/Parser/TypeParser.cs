#nullable enable

using Deimos.AST;
using Deimos.AST.Types;
using Deimos.Exceptions;
using Deimos.Lexer;
using System.Collections.Generic;

namespace Deimos.Parser
{
    public sealed partial class Parser
    {
        public TypeNode ParseType() => ParseTypeImpl(genericLevel: 0);

        public bool TryParseType(out TypeNode type)
        {
            int initialPosition = _reader.Position;
            try
            {
                type = ParseTypeImpl(genericLevel: 0);
                return true;
            }
            catch (DeimosParserException)
            {
                _reader.Position = initialPosition;
                type = null!;
                return false;
            }
        }

        public bool TryParseNonAmbiguousType(out TypeNode type)
        {
            int initialPosition = _reader.Position;
            if (!TryParseType(out type))
                return false;

            // Check for ambiguity: identifier or nested (and generic nested) types are ambiguous
            if (type is NamedTypeNode or NestedNamedTypeNode or NestedGenericTypeNode)
            {
                _reader.Position = initialPosition;
                type = null!;
                return false;
            }

            return true;
        }

        private TypeNode ParseTypeImpl(uint genericLevel)
        {
            var type = ParsePrimaryType(genericLevel);
            while (true)
            {
                // Array type: T[]
                if (_reader.Match(TokenType.LeftBracket))
                {
                    _reader.Expect(TokenType.RightBracket, "Expected ']' to close array type.");
                    type = new ArrayTypeNode(type, type.From, _reader.CurrentEndIndex);
                    continue;
                }

                // Nullable type: T?
                if (_reader.Match(TokenType.Question))
                {
                    type = new NullableTypeNode(type, type.From, _reader.CurrentEndIndex);
                    continue;
                }

                break;
            }

            return type;
        }

        private TypeNode ParsePrimaryType(uint genericLevel)
        {
            var initialIndex = _reader.CurrentStartIndex;
            Token token = _reader.Current;

            // 1. Dictionary type: { K : V }
            if (_reader.Match(TokenType.LeftBrace))
            {
                var keyType = ParseTypeImpl(genericLevel);
                _reader.Expect(TokenType.Colon, "Expected ':' in dictionary type.");

                var valueType = ParseTypeImpl(genericLevel);
                _reader.Expect(TokenType.RightBrace, "Expected '}' to close dictionary type.");

                return new DictionaryTypeNode(keyType, valueType, initialIndex, _reader.CurrentEndIndex);
            }

            // 2. Tuple or function type: ( T1, T2, ... ) or ( T1, T2, ... ): R
            if (_reader.Match(TokenType.LeftParen))
            {
                // Empty tuple (,)
                if (_reader.Match(TokenType.Comma))
                {
                    _reader.Expect(TokenType.RightParen, "Expected ')' to close tuple type.");
                    return new TupleTypeNode(System.Array.Empty<TypeNode>(), initialIndex, _reader.CurrentEndIndex);
                }

                bool extraComma = false;
                var types = new List<TypeNode>();
                if (_reader.Current.Type != TokenType.RightParen)
                {
                    while (true)
                    {
                        types.Add(ParseTypeImpl(genericLevel));
                        if (_reader.Match(TokenType.Comma))
                        {
                            if (_reader.Current.Type != TokenType.RightParen)
                                continue;
                            extraComma = true;
                        }
                        break;
                    }
                }
                _reader.Expect(TokenType.RightParen, "Expected ')' to close tuple or function type.");

                if (_reader.Match(TokenType.Colon))
                {
                    if (extraComma)
                        throw new DeimosParserException("Unexpected ',' before return type in function type.", initialIndex, _reader.CurrentEndIndex);

                    var returnType = ParseTypeImpl(genericLevel);
                    return new FunctionTypeNode(returnType, types, initialIndex, _reader.CurrentEndIndex);
                }
                else
                {
                    if (!extraComma && types.Count < 2)
                        throw new DeimosParserException("Tuple type must have at least two element types or end with a ','.", initialIndex, _reader.CurrentEndIndex);
                    return new TupleTypeNode(types, initialIndex, _reader.CurrentEndIndex);
                }
            }

            // 3. Primitive type: int, float, bool, string, void, any
            if (IsPrimitiveTypeToken(token))
            {
                _reader.Advance();
                if (token.Type == TokenType.Void)
                    return new VoidTypeNode(token.From, _reader.CurrentEndIndex);
                if (token.Type == TokenType.Any)
                    return new AnyTypeNode(token.From, _reader.CurrentEndIndex);
                return new PrimitiveTypeNode(TokenToPrimitiveType(token), token.From, _reader.CurrentEndIndex);
            }

            // 3. Identifier or generic type: MyType, MyType<T1, T2>
            if (_reader.Match(TokenType.Identifier))
                return ParseIdentifierTypeNode(genericLevel);

            throw new DeimosParserException("Expected a type.", token.From, token.To);
        }

        private TypeNode ParseIdentifierTypeNode(uint genericLevel)
        {
            if (!_reader.Check(TokenType.Identifier))
                throw new DeimosParserException("Expected identifier for type.", _reader.Current.From, _reader.Current.To);

            TypeNode? type = null;

            while (true)
            {
                var token = _reader.Current;
                _reader.Advance();

                var typeName = token.Lexeme;

                // Generic type: MyType<T1, T2>
                if (_reader.Match(TokenType.Less))
                {
                    var typeArguments = new List<TypeNode>();
                    if (_reader.Current.Type != TokenType.Greater && (genericLevel % 2 == 0 || _reader.Current.Type != TokenType.ShiftRight))
                    {
                        while (true)
                        {
                            typeArguments.Add(ParseTypeImpl(genericLevel + 1));
                            if (_reader.Match(TokenType.Comma))
                                continue;
                            break;
                        }
                    }
                    // Expect closing '>' or '>>' based on generic level
                    if (genericLevel % 2 == 0) // even -> expect '>' to close generic type arguments
                        _reader.Expect(TokenType.Greater, "Expected '>' to close generic type arguments.");
                    else // odd -> expect '>>' or '>' to close generic type arguments
                    {
                        if (_reader.Current.Type != TokenType.Greater && _reader.Current.Type != TokenType.ShiftRight)
                            throw new DeimosParserException("Expected '>' to close generic type arguments.", _reader.Current.From, _reader.Current.To);
                    }

                    if (type is null)
                        type = new GenericTypeNode(typeName, typeArguments, token.From, _reader.CurrentEndIndex);
                    else
                        type = new NestedGenericTypeNode(type, typeName, typeArguments, type.From, _reader.CurrentEndIndex);

                    // Handle nested generic types: MyType<T1, T2>.NestedType
                    if (_reader.Match(TokenType.Dot))
                    {
                        _reader.Expect(TokenType.Identifier, "Expected identifier for nested type.");
                        continue;
                    }

                    return type;
                }

                if (type is null)
                    type = new NamedTypeNode(typeName, token.From, _reader.CurrentEndIndex);
                else
                    type = new NestedNamedTypeNode(type, typeName, type.From, _reader.CurrentEndIndex);

                // Handle nested types: MyType.NestedType
                if (_reader.Match(TokenType.Dot))
                {
                    _reader.Expect(TokenType.Identifier, "Expected identifier for nested type.");
                    continue;
                }

                return type;
            }
        }

        private bool IsPrimitiveTypeToken(Token token) => token.Type switch
        {
            TokenType.Int or TokenType.Float or TokenType.Bool or TokenType.String or TokenType.Void or TokenType.Any => true,
            _ => false,
        };

        private PrimitiveType TokenToPrimitiveType(Token token) => token.Type switch
        {
            TokenType.Int => PrimitiveType.Int,
            TokenType.Float => PrimitiveType.Float,
            TokenType.Bool => PrimitiveType.Bool,
            TokenType.String => PrimitiveType.String,
            _ => throw new DeimosParserException($"Token '{token.Type}' is not a primitive type.", token.From, token.To),
        };
    }
}
