#nullable enable

using Deimos.Lexer;

namespace Deimos.AST
{
    public sealed class VoidTypeNode : TypeNode
    {
        public VoidTypeNode(TextRange range) : base(range) {}

        public VoidTypeNode(TextIndex from, TextIndex to) : base(from, to) {}

        public VoidTypeNode(int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {}

        public override string ToString()
        {
            return "void";
        }
    }
}
