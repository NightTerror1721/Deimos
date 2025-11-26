#nullable enable

using Deimos.AST.Declarations;
using Deimos.Lexer;
using Deimos.Utils;

namespace Deimos.AST.Statements
{
    public sealed class VariableDeclarationStatement : Statement
    {
        public VariableDeclarationKind VarKind { get; }
        public TypeNode? Type { get; }
        public string Name { get; }
        public Expression? Initializer { get; }

        public bool IsConstant => VarKind == VariableDeclarationKind.Const;
        public bool HasExplicitVarKindKeyword => VarKind != VariableDeclarationKind.Default;
        public bool HasExplicitType => Type != null;
        public bool HasInitializer => Initializer != null;

        public VariableDeclarationStatement(
            VariableDeclarationKind varKind,
            string name,
            TypeNode? type,
            Expression? initializer,
            TextRange range
        ) : base(range)
        {
            VarKind = varKind;
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Type = type;
            Initializer = initializer;
        }

        public VariableDeclarationStatement(
            VariableDeclarationKind varKind,
            string name,
            TypeNode? type,
            Expression? initializer,
            TextIndex from,
            TextIndex to
        ) : base(from, to)
        {
            VarKind = varKind;
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Type = type;
            Initializer = initializer;
        }

        public VariableDeclarationStatement(
            VariableDeclarationKind varKind,
            string name,
            TypeNode? type,
            Expression? initializer,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(fromLine, fromColumn, toLine, toColumn)
        {
            VarKind = varKind;
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Type = type;
            Initializer = initializer;
        }

        public sealed override string ToString(Indentation indent)
        {
            string varKindStr = VarKind switch
            {
                VariableDeclarationKind.Var => "var ",
                VariableDeclarationKind.Const => "const ",
                _ => string.Empty
            };
            string typeStr = Type != null ? $"{Type} " : string.Empty;
            string initializerStr = Initializer != null ? $" = {Initializer}" : "";
            return $"{varKindStr}{typeStr}{Name}{initializerStr};";
        }
    }
}
