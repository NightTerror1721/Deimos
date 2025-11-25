#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class StaticMemberAccessExpression : Expression
    {
        public TypeNode Target { get; }
        public string MemberName { get; }

        public StaticMemberAccessExpression(TypeNode target, string memberName, TextRange range) : base(range)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
            MemberName = memberName ?? throw new System.ArgumentNullException(nameof(memberName));
        }

        public StaticMemberAccessExpression(TypeNode target, string memberName, TextIndex from, TextIndex to) : base(from, to)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
            MemberName = memberName ?? throw new System.ArgumentNullException(nameof(memberName));
        }

        public StaticMemberAccessExpression(TypeNode target, string memberName, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
            MemberName = memberName ?? throw new System.ArgumentNullException(nameof(memberName));
        }

        public override string ToString()
        {
            return $"{Target}.{MemberName}";
        }
    }
}
