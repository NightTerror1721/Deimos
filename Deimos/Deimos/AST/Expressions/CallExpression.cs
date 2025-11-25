#nullable enable

using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;

namespace Deimos.AST.Expressions
{
    public sealed class CallExpression : Expression
    {
        public Expression Callee { get; }
        public ReadOnlyArray<Expression> Arguments { get; }

        public CallExpression(Expression callee, IEnumerable<Expression> arguments, TextRange range) : base(range)
        {
            Callee = callee ?? throw new System.ArgumentNullException(nameof(callee));
            Arguments = arguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(arguments));
        }

        public CallExpression(Expression callee, IEnumerable<Expression> arguments, TextIndex from, TextIndex to) : base(from, to)
        {
            Callee = callee ?? throw new System.ArgumentNullException(nameof(callee));
            Arguments = arguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(arguments));
        }

        public CallExpression(Expression callee, IEnumerable<Expression> arguments, int fromLine, int fromColumn, int toLine, int toColumn) :
            base(fromLine, fromColumn, toLine, toColumn)
        {
            Callee = callee ?? throw new System.ArgumentNullException(nameof(callee));
            Arguments = arguments?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(arguments));
        }

        public override string ToString()
        {
            if (Arguments.Count == 0)
                return $"{Callee}()";
            return $"{Callee}({string.Join(", ", Arguments)})";
        }
    }
}
