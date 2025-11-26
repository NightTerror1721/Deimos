using Deimos.AST.Declarations;
using Deimos.Lexer;
using Deimos.Utils;
using System.Collections.Generic;
using System.Text;

namespace Deimos.AST.Private
{
    public abstract class AbstractClassDeclaration : Declaration
    {
        public string Name { get; }
        public ReadOnlyArray<TypeParameterNode> TypeParameterNodes { get; }
        public ReadOnlyArray<TypeNode> Parents { get; }
        public ReadOnlyArray<Declaration> Members { get; }

        public AbstractClassDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameterNodes,
            IEnumerable<TypeNode> parents,
            IEnumerable<Declaration> members,
            TextRange range
        ) : base(modifiers, range)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeParameterNodes = typeParameterNodes?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeParameterNodes));
            Parents = parents?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(parents));
            Members = members?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(members));
        }

        public AbstractClassDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameterNodes,
            IEnumerable<TypeNode> parents,
            IEnumerable<Declaration> members,
            TextIndex from,
            TextIndex to
        ) : base(modifiers, from, to)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeParameterNodes = typeParameterNodes?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeParameterNodes));
            Parents = parents?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(parents));
            Members = members?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(members));
        }

        public AbstractClassDeclaration(
            string name,
            Modifiers modifiers,
            IEnumerable<TypeParameterNode> typeParameterNodes,
            IEnumerable<TypeNode> parents,
            IEnumerable<Declaration> members,
            int fromLine,
            int fromColumn,
            int toLine,
            int toColumn
        ) : base(modifiers, fromLine, fromColumn, toLine, toColumn)
        {
            Name = name ?? throw new System.ArgumentNullException(nameof(name));
            TypeParameterNodes = typeParameterNodes?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(typeParameterNodes));
            Parents = parents?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(parents));
            Members = members?.ToReadOnlyArray() ?? throw new System.ArgumentNullException(nameof(members));
        }

        public sealed override string ToString(Indentation indent)
        {
            var sb = new StringBuilder();
            sb.Append($"{Modifiers.ToKeywordString()} {ClassDeclarationTypeName} {Name}");
            if (TypeParameterNodes.Count > 0)
            {
                sb.Append("<");
                for (int i = 0; i < TypeParameterNodes.Count; i++)
                {
                    sb.Append(TypeParameterNodes[i].ToString());
                    if (i < TypeParameterNodes.Count - 1)
                        sb.Append(", ");
                }
                sb.Append(">");
            }

            if (Parents.Count > 0)
            {
                sb.Append(" : ");
                for (int i = 0; i < Parents.Count; i++)
                {
                    sb.Append(Parents[i].ToString());
                    if (i < Parents.Count - 1)
                        sb.Append(", ");
                }
            }

            sb.AppendLine();
            sb.AppendLine($"{indent}{{");

            Indentation newIndent = indent.Increase();
            string newIndentStr = newIndent.ToString();
            foreach (var member in Members)
                sb.Append(newIndentStr).AppendLine(member.ToString(newIndent));

            sb.AppendLine($"{indent}}}");

            return sb.ToString();
        }
        private protected abstract string ClassDeclarationTypeName { get; }
    }
}
