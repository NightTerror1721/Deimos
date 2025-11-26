#nullable enable

using Deimos.AST.Declarations;
using Deimos.Lexer;
using Deimos.Utils;

namespace Deimos.AST.Private
{
    public abstract class AbstractFieldDeclaration : Declaration
    {
        public VariableDeclarationKind VarKind { get; }
        public TypeNode? Type { get; }
        public string Name { get; }
        public Expression? Initializer { get; }

        public bool IsConstant => VarKind == VariableDeclarationKind.Const;
        public bool HasExplicitVarKindKeyword => VarKind != VariableDeclarationKind.Default;
        public bool HasExplicitType => Type != null;
        public bool HasInitializer => Initializer != null;

        public AbstractFieldDeclaration(
            VariableDeclarationKind varKind,
            string name,
            TypeNode? type,
            Expression? initializer,
            Modifiers modifiers,
            TextRange range
        ) : base(modifiers, range)
        {
            VarKind = varKind;
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Type = type;
            Initializer = initializer;
        }

        public AbstractFieldDeclaration(
            VariableDeclarationKind varKind,
            string name,
            TypeNode? type,
            Expression? initializer,
            Modifiers modifiers,
            TextIndex from,
            TextIndex to
        ) : base(modifiers, from, to)
        {
            VarKind = varKind;
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            Type = type;
            Initializer = initializer;
        }

        public AbstractFieldDeclaration(
            VariableDeclarationKind varKind,
            string name,
            TypeNode? type,
            Expression? initializer,
            Modifiers modifiers,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(modifiers, fromLine, fromColumn, toLine, toColumn)
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
            return $"{Modifiers.ToKeywordString()} {varKindStr}{typeStr}{Name}{initializerStr};";
        }
    }
}
