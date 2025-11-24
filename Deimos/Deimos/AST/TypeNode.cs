#nullable enable

using Deimos.Lexer;

namespace Deimos.AST
{
    public abstract class TypeNode : Node
    {
        protected TypeNode(TextRange range) : base(range) { }

        protected TypeNode(TextIndex from, TextIndex to) : base(from, to) { }

        protected TypeNode(int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        { }
    }
}
