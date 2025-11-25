#nullable enable

using Deimos.Lexer;
using Deimos.Utils;

namespace Deimos.AST
{
    public abstract class TypeNode : Node
    {
        protected TypeNode(TextRange range) : base(range) { }

        protected TypeNode(TextIndex from, TextIndex to) : base(from, to) { }

        protected TypeNode(int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        { }

        public abstract override string ToString();
        public sealed override string ToString(Indentation indent) => ToString();
    }
}
