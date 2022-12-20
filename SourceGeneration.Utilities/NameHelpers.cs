using System.Text;

namespace CodeChops.SourceGeneration.Utilities;

/// <summary>
/// Provides helpers for class (or interface) names.
/// </summary>
public static class NameHelpers
{
	/// <summary>
	/// Gets the name without generic parameters.
	/// </summary>
	public static string GetNameWithoutGenerics(string className)
	{
		var genericParameterIndex = className.IndexOf('<');
		var classNameWithoutGenerics = genericParameterIndex <= 0 ? className : className.Substring(0, genericParameterIndex);

		return classNameWithoutGenerics;
	}
	
	/// <summary>
	/// Gets the generic parameters.
	/// </summary>
	public static string? GetGenericsParameters(string className)
	{
		var genericParametersEndIndex = className.IndexOf('<');
		if (genericParametersEndIndex < 0)
			return null;
		
		var genericParameters = className.Substring(genericParametersEndIndex);

		return genericParameters;
	}

	/// <summary>
	/// Checks if a name contains a generic parameter.
	/// </summary>
	public static bool HasGenericParameter(string className)
	{
		var hasGenericParameter = className.Contains('<');
		return hasGenericParameter;
	}
	
	/// <summary>
	/// Gets the name of a class including the generic types.
	/// <para>
	/// E.g: System.Collections.Generic.Dictionary`2 becomes System.Collections.Generic.Dictionary&lt;System.String,System.Object&gt;.
	/// </para>
	/// </summary>
	public static string GetNameWithGenerics(Type type)
	{
		if (type.IsGenericParameter)
			return type.Name;

		if (!type.IsGenericType)
			return type.FullName ?? type.Name;

		var builder = new StringBuilder();
		var name = type.Name;
		var index = name.IndexOf("`", StringComparison.Ordinal);
		builder.AppendFormat("{0}.{1}", type.Namespace, name.Substring(0, index));
		builder.Append('<');
		var first = true;
		foreach (var arg in type.GetGenericArguments())
		{
			if (!first)
			{
				builder.Append(',');
			}
			builder.Append(GetNameWithGenerics(arg));
			first = false;
		}
		builder.Append('>');
		return builder.ToString();
	}
}
