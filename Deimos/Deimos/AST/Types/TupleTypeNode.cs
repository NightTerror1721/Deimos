#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;

namespace Deimos.AST.Types
{
    public sealed class TupleTypeNode : TypeNode
    {
        public ReadOnlyArray<TypeNode> ElementTypes { get; }

        public TupleTypeNode(IEnumerable<TypeNode> elementTypes, TextRange range) : base(range)
        {
            ElementTypes = elementTypes?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(elementTypes));
        }

        public TupleTypeNode(IEnumerable<TypeNode> elementTypes, TextIndex from, TextIndex to) : base(from, to)
        {
            ElementTypes = elementTypes?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(elementTypes));
        }

        public TupleTypeNode(IEnumerable<TypeNode> elementTypes, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            ElementTypes = elementTypes?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(elementTypes));
        }

        public override string ToString()
        {
            if (ElementTypes.Length == 0)
                return "()";

            if (ElementTypes.Length == 1)
                return $"({ElementTypes[0]},)";

            var elements = string.Join(", ", ElementTypes);
            return $"({elements})";
        }
    }
}
