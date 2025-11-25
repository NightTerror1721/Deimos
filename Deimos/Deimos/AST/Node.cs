#nullable enable

using Deimos.Lexer;
using Deimos.Utils;

namespace Deimos.AST
{
    public abstract class Node
    {
        public TextRange Range { get; }

        public TextIndex From => Range.Start;
        public TextIndex To => Range.End;

        public int FromLine => Range.Start.Line;
        public int FromColumn => Range.Start.Column;
        public int ToLine => Range.End.Line;
        public int ToColumn => Range.End.Column;

        protected Node(TextRange range)
        {
            Range = range;
        }

        protected Node(TextIndex from, TextIndex to) : this(TextRange.From(from, to)) { }

        protected Node(int fromLine, int fromColumn, int toLine, int toColumn) :
            this(TextRange.From(fromLine, fromColumn, toLine, toColumn)) { }

        public override string ToString() => ToString(Indentation.None);
        public abstract string ToString(Indentation indent);
    }
}
