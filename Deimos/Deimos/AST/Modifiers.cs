#nullable enable

using System;
using System.Collections.Generic;

namespace Deimos.AST
{
    [Flags]
    public enum Modifiers
    {
        None        = 0,
        Static      = 1 << 0,
        Sealed      = 1 << 1,
        Abstract    = 1 << 2,
        Override    = 1 << 3,
        Public      = 1 << 4,
        Protected   = 1 << 5,
        Private     = 1 << 6,
    }

    public static class ModifierUtils
    {
        public static bool IsAccessModifier(this Modifiers modifier)
        {
            return modifier == Modifiers.Public ||
                   modifier == Modifiers.Protected ||
                   modifier == Modifiers.Private;
        }

        public static bool IsNonAccessModifier(this Modifiers modifier)
        {
            return modifier == Modifiers.Static ||
                   modifier == Modifiers.Sealed ||
                   modifier == Modifiers.Abstract ||
                   modifier == Modifiers.Override;
        }

        public static bool IsStatic(this Modifiers modifier) => modifier.HasModifier(Modifiers.Static);
        public static bool IsSealed(this Modifiers modifier) => modifier.HasModifier(Modifiers.Sealed);
        public static bool IsAbstract(this Modifiers modifier) => modifier.HasModifier(Modifiers.Abstract);
        public static bool IsOverride(this Modifiers modifier) => modifier.HasModifier(Modifiers.Override);
        public static bool IsPublic(this Modifiers modifier) => modifier.HasModifier(Modifiers.Public);
        public static bool IsProtected(this Modifiers modifier) => modifier.HasModifier(Modifiers.Protected);
        public static bool IsPrivate(this Modifiers modifier) => modifier.HasModifier(Modifiers.Private);

        public static bool HasAnyAccessModifier(this Modifiers modifiers)
        {
            return modifiers.HasModifier(Modifiers.Public) ||
                   modifiers.HasModifier(Modifiers.Protected) ||
                   modifiers.HasModifier(Modifiers.Private);
        }

        public static bool HasModifier(this Modifiers modifiers, Modifiers modifier)
        {
            return (modifiers & modifier) == modifier;
        }

        public static Modifiers Add(this Modifiers modifiers, Modifiers modifier) => modifiers | modifier;
        public static Modifiers Remove(this Modifiers modifiers, Modifiers modifier) => modifiers & ~modifier;

        public static string ToKeywordString(this Modifiers modifiers)
        {
            string result = "";
            if (modifiers.HasModifier(Modifiers.Public))
                result += "public ";
            else if (modifiers.HasModifier(Modifiers.Protected))
                result += "protected ";
            else if (modifiers.HasModifier(Modifiers.Private))
                result += "private ";
            if (modifiers.HasModifier(Modifiers.Static))
                result += "static ";
            if (modifiers.HasModifier(Modifiers.Sealed))
                result += "sealed ";
            if (modifiers.HasModifier(Modifiers.Abstract))
                result += "abstract ";
            if (modifiers.HasModifier(Modifiers.Override))
                result += "override ";
            return result.TrimEnd();
        }

        public static bool HasInvalidCombination(this Modifiers modifiers)
        {
            int accessModifierCount = 0;
            if (modifiers.HasModifier(Modifiers.Public)) accessModifierCount++;
            if (modifiers.HasModifier(Modifiers.Protected)) accessModifierCount++;
            if (modifiers.HasModifier(Modifiers.Private)) accessModifierCount++;
            if (accessModifierCount > 1)
                return true;

            if (modifiers.HasModifier(Modifiers.Static))
            {
                if (modifiers.HasModifier(Modifiers.Abstract) || modifiers.HasModifier(Modifiers.Override) || modifiers.HasModifier(Modifiers.Sealed))
                    return true;
            }

            if (modifiers.HasModifier(Modifiers.Abstract) && modifiers.HasModifier(Modifiers.Sealed))
                return true;

            return false;
        }

        public static void CheckValidCombination(this Modifiers modifiers)
        {
            int accessModifierCount = 0;
            if (modifiers.HasModifier(Modifiers.Public)) accessModifierCount++;
            if (modifiers.HasModifier(Modifiers.Protected)) accessModifierCount++;
            if (modifiers.HasModifier(Modifiers.Private)) accessModifierCount++;
            if (accessModifierCount > 1)
                throw new InvalidOperationException("Multiple access modifiers are not allowed.");

            if (modifiers.HasModifier(Modifiers.Static))
            {
                if (modifiers.HasModifier(Modifiers.Abstract) || modifiers.HasModifier(Modifiers.Override) || modifiers.HasModifier(Modifiers.Sealed))
                    throw new InvalidOperationException("'static' modifier cannot be combined with 'abstract', 'override', or 'sealed' modifiers.");
            }

            if (modifiers.HasModifier(Modifiers.Abstract) && modifiers.HasModifier(Modifiers.Sealed))
                throw new InvalidOperationException("'abstract' and 'sealed' modifiers cannot be combined.");
        }

        public static IEnumerable<Modifiers> GetIndividualModifiers(this Modifiers modifiers)
        {
            foreach (Modifiers modifier in Enum.GetValues(typeof(Modifiers)))
                if (modifier != Modifiers.None && modifiers.HasModifier(modifier))
                    yield return modifier;
        }
    }
}
