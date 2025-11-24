#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Types
{
    public sealed class DictionaryTypeNode : TypeNode
    {
        public TypeNode KeyType { get; }
        public TypeNode ValueType { get; }

        public DictionaryTypeNode(TypeNode keyType, TypeNode valueType, TextRange range) : base(range)
        {
            KeyType = keyType ?? throw new System.ArgumentNullException(nameof(keyType));
            ValueType = valueType ?? throw new System.ArgumentNullException(nameof(valueType));
        }

        public DictionaryTypeNode(TypeNode keyType, TypeNode valueType, TextIndex from, TextIndex to) : base(from, to)
        {
            KeyType = keyType ?? throw new System.ArgumentNullException(nameof(keyType));
            ValueType = valueType ?? throw new System.ArgumentNullException(nameof(valueType));
        }

        public DictionaryTypeNode(TypeNode keyType, TypeNode valueType, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            KeyType = keyType ?? throw new System.ArgumentNullException(nameof(keyType));
            ValueType = valueType ?? throw new System.ArgumentNullException(nameof(valueType));
        }

        public override string ToString()
        {
            return $"{{ {KeyType} : {ValueType} }}";
        }
    }
}
