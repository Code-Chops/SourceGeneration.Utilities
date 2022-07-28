namespace CodeChops.SourceGeneration.Utilities.Extensions;

public static class NameSyntaxExtensions
{
	/// <summary>
	/// Checks if a name syntax has a specific attribute.
	/// </summary>
	/// <param name="expectedGenericTypeParams">Add these parameters to check if the generic type parameters are matching. Use NULL to match any value.</param>
	public static bool HasAttributeName(this NameSyntax? name, string expectedName, CancellationToken cancellationToken, IEnumerable<string>? expectedGenericTypeParams = default)
	{
		var attributeName = name.ExtractAttributeName(cancellationToken, out var genericTypeParams);

		if (attributeName is null) return false;
		if (attributeName == expectedName) return true;
		
		var alternativeAttributeName = attributeName.EndsWith("Attribute")
		   ? attributeName.Substring(0, attributeName.Length - "Attribute".Length)
		   : $"{attributeName}Attribute";

		if (alternativeAttributeName != expectedName) return false;

		if (expectedGenericTypeParams is null || !expectedGenericTypeParams.Any()) return true;
		
		var correctParams = expectedGenericTypeParams.All(param => param is null || genericTypeParams.Contains(param));
		return correctParams;
	}

	/// <summary>
	/// Extracts the attribute name from the name syntax.
	/// </summary>
	/// <returns>The attribute name</returns>
	public static string? ExtractAttributeName(this NameSyntax? name, CancellationToken cancellationToken, out IEnumerable<string> genericTypeParams)
	{
		while (name != null && !cancellationToken.IsCancellationRequested)
		{
			switch (name)
			{
				case QualifiedNameSyntax qualifiedName:
					name = qualifiedName.Right;
					break;
				
				case IdentifierNameSyntax identifierName:
					genericTypeParams = Array.Empty<string>();
					return identifierName.Identifier.Text;

				case GenericNameSyntax genericNameSyntax:
					genericTypeParams = genericNameSyntax.TypeArgumentList.Arguments.Select(argument => argument.ToString());
					return genericNameSyntax.Identifier.Text;
				
				default:
					genericTypeParams = Array.Empty<string>();
					return null;
			}
		}

		genericTypeParams = Array.Empty<string>();
		return null;
	}
}