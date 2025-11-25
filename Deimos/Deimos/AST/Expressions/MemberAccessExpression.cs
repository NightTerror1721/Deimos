#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Expressions
{
    public sealed class MemberAccessExpression : Expression
    {
        public Expression Target { get; }
        public string MemberName { get; }
        public bool IsSafeAccess { get; }

        public MemberAccessExpression(Expression target, string memberName, bool isSafeAccess, TextRange range) : base(range)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
            MemberName = memberName ?? throw new System.ArgumentNullException(nameof(memberName));
            IsSafeAccess = isSafeAccess;
        }
        public MemberAccessExpression(Expression target, string memberName, bool isSafeAccess, TextIndex from, TextIndex to) : base(from, to)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
            MemberName = memberName ?? throw new System.ArgumentNullException(nameof(memberName));
            IsSafeAccess = isSafeAccess;
        }
        public MemberAccessExpression(Expression target, string memberName, bool isSafeAccess, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Target = target ?? throw new System.ArgumentNullException(nameof(target));
            MemberName = memberName ?? throw new System.ArgumentNullException(nameof(memberName));
            IsSafeAccess = isSafeAccess;
        }

        public override string ToString()
        {
            if (IsSafeAccess)
                return $"{Target}?.{MemberName}";
            return $"{Target}.{MemberName}";
        }
    }
}
