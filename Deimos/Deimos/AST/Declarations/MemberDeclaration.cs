#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Declarations
{
    public abstract class MemberDeclaration : Declaration
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

        protected MemberDeclaration(Modifiers modifiers, TextRange range) : base(range)
        {
            Modifiers = modifiers;
        }

        protected MemberDeclaration(Modifiers modifiers, TextIndex from, TextIndex to) : base(from, to)
        {
            Modifiers = modifiers;
        }

        protected MemberDeclaration(Modifiers modifiers, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Modifiers = modifiers;
        }
    }
}
