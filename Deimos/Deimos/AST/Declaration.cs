#nullable enable

using Deimos.Lexer;

namespace Deimos.AST
{
    public abstract class Declaration : Node
    {
        public Modifiers Modifiers { get; }

        public bool IsStatic => Modifiers.IsStatic();
        public bool IsSealed => Modifiers.IsSealed();
        public bool IsAbstract => Modifiers.IsAbstract();
        public bool IsOverride => Modifiers.IsOverride();
        public bool IsPublic => Modifiers.IsPublic();
        public bool IsProtected => Modifiers.IsProtected();
        public bool IsPrivate => Modifiers.IsPrivate();

        public bool HasAccessModifier => Modifiers.HasAnyAccessModifier();

        protected Declaration(Modifiers modifiers, TextRange range) : base(range)
        {
            Modifiers = modifiers;
        }

        protected Declaration(Modifiers modifiers, TextIndex from, TextIndex to) : base(from, to)
        {
            Modifiers = modifiers;
        }

        protected Declaration(Modifiers modifiers, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Modifiers = modifiers;
        }
    }
}
