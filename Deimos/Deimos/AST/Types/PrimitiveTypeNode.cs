#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Types
{
    public sealed class PrimitiveTypeNode : TypeNode
    {
        public PrimitiveType PrimitiveType { get; }

        public PrimitiveTypeNode(PrimitiveType primitiveType, TextRange range) : base(range)
        {
            PrimitiveType = primitiveType;
        }

        public PrimitiveTypeNode(PrimitiveType primitiveType, TextIndex from, TextIndex to) : base(from, to)
        {
            PrimitiveType = primitiveType;
        }

        public PrimitiveTypeNode(PrimitiveType primitiveType, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            PrimitiveType = primitiveType;
        }

        public override string ToString() => PrimitiveType switch
        {
            PrimitiveType.Int => "int",
            PrimitiveType.Float => "float",
            PrimitiveType.Bool => "bool",
            PrimitiveType.String => "string",
            _ => "unknown"
        };
    }

    public enum PrimitiveType
    {
        Int,
        Float,
        Bool,
        String
    }
}
