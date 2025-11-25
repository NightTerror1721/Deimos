#nullable enable

using Deimos.Lexer;
using Deimos.Utils;

namespace Deimos.AST
{
    public sealed class TypeParameterNode : Node
    {
        public string Name { get; }

        public TypeParameterNode(string name, TextRange range) : base(range)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        public TypeParameterNode(string name, TextIndex from, TextIndex to) : base(from, to)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        public TypeParameterNode(string name, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
        }

        public override string ToString()
        {
            return Name;
        }

        public override string ToString(Indentation indent)
        {
            return Name;
        }
    }
}
