using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseR;

internal static class TypeExtensions
{
    [RequiresDynamicCode("This code require for debug asserts.")]
    [RequiresUnreferencedCode("This code require for debug asserts.")]
    public static bool IsAssignableToOpenGenericInterface(this Type type, Type otherInterfaceType)
    {
        int argumentsCount = type.GetGenericArguments().Length;
        if (argumentsCount != otherInterfaceType.GetGenericArguments().Length)
        {
            return false;
        }

        Type[] genericArgs = new Type[argumentsCount];
        Array.Fill(genericArgs, typeof(object));

        return type.MakeGenericType(typeArguments: genericArgs).IsAssignableTo(otherInterfaceType.MakeGenericType(typeArguments: genericArgs));
    }
}
