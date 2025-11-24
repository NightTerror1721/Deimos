#nullable enable

using Deimos.Lexer;

namespace Deimos.AST
{
    public sealed class AnyTypeNode : TypeNode
    {
        public AnyTypeNode(TextRange range) : base(range) {}

        public AnyTypeNode(TextIndex from, TextIndex to) : base(from, to) {}

        public AnyTypeNode(int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {}

        public override string ToString()
        {
            return "any";
        }
    }
}
