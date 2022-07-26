namespace CodeChops.SourceGeneration.Utilities;

/// <summary>
/// Provides helpers for class (or interface) names.
/// </summary>
public static class ClassNameHelpers
{
	public static string GetClassNameWithoutGenerics(string className)
	{
		var genericParameterIndex = className.IndexOf('<');
		var classNameWithoutGenerics = genericParameterIndex <= 0 ? className : className.Substring(0, genericParameterIndex);

		return classNameWithoutGenerics;
	}

	public static bool HasGenericParameter(string className)
	{
		var hasGenericParameter = className.Contains('<');
		return hasGenericParameter;
	}
}