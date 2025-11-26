#nullable enable

using Deimos.Lexer;

namespace Deimos.AST.Declarations
{
    public sealed class FieldDeclaration : Private.AbstractFieldDeclaration
    {
        public FieldDeclaration(
            VariableDeclarationKind varKind,
            string name,
            TypeNode? type,
            Expression? initializer,
            Modifiers modifiers,
            TextRange range
        ) : base(varKind, name, type, initializer, modifiers, range) { }

        public FieldDeclaration(
            VariableDeclarationKind varKind,
            string name,
            TypeNode? type,
            Expression? initializer,
            Modifiers modifiers,
            TextIndex from,
            TextIndex to
        ) : base(varKind, name, type, initializer, modifiers, from, to) { }

        public FieldDeclaration(
            VariableDeclarationKind varKind,
            string name,
            TypeNode? type,
            Expression? initializer,
            Modifiers modifiers,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(varKind, name, type, initializer, modifiers, fromLine, fromColumn, toLine, toColumn) { }
    }

    public enum VariableDeclarationKind
    {
        Default = 0,
        Var,
        Const
    }
}
