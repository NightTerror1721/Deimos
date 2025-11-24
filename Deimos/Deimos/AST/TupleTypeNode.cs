#nullable enable

using Deimos.Lexer;
using System.Collections.Generic;
using System.Linq;

namespace Deimos.AST
{
    public sealed class TupleTypeNode : TypeNode
    {
        public TypeNode[] ElementTypes { get; }

        public TupleTypeNode(IEnumerable<TypeNode> elementTypes, TextRange range) : base(range)
        {
            ElementTypes = elementTypes?.ToArray() ?? throw new System.ArgumentNullException(nameof(elementTypes));
        }

        public TupleTypeNode(TypeNode[] elementTypes, TextIndex from, TextIndex to) : base(from, to)
        {
            ElementTypes = elementTypes?.ToArray() ?? throw new System.ArgumentNullException(nameof(elementTypes));
        }

        public TupleTypeNode(TypeNode[] elementTypes, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            ElementTypes = elementTypes?.ToArray() ?? throw new System.ArgumentNullException(nameof(elementTypes));
        }

        public override string ToString()
        {
            if (ElementTypes.Length == 0)
                return "()";

            if (ElementTypes.Length == 1)
                return $"({ElementTypes[0]},)";

            var elements = string.Join(", ", (IEnumerable<TypeNode>)ElementTypes);
            return $"({elements})";
        }
    }
}
