namespace CodeChops.SourceGeneration.Utilities.Extensions;

public static class NameSyntaxExtensions
{
	/// <summary>
	/// Checks if a name syntax has a specific attribute.
	/// </summary>
	public static bool HasAttributeName(this NameSyntax? name, string expectedName, CancellationToken cancellationToken)
	{
		var expectedGenericTypeParams = name is GenericNameSyntax genericName   
			? genericName.TypeArgumentList.Arguments.Select(arg => arg.ToString())
			: null;
		
		var attributeName = name.ExtractAttributeName(cancellationToken, out var genericTypeParams);
		
		var hasAttributeName = NameIsCorrect() && HasCorrectGenericParameters();
		return hasAttributeName;
		
		
		bool NameIsCorrect()
		{
			if (attributeName is null) return false;
			if (attributeName == expectedName) return true;
		
			var alternativeAttributeName = attributeName.EndsWith("Attribute")
				? attributeName.Substring(0, attributeName.Length - "Attribute".Length)
				: $"{attributeName}Attribute";

			return alternativeAttributeName == expectedName;
		}

		bool HasCorrectGenericParameters()
		{
			if (expectedGenericTypeParams is null) return true;
			
			if (genericTypeParams.Count() != expectedGenericTypeParams.Count()) return false;
		
			var correctParams = expectedGenericTypeParams.All(param => param is null || genericTypeParams.Contains(param));
			return correctParams;
		}
	}

	/// <summary>
	/// Extracts the attribute name from the name syntax.
	/// </summary>
	/// <returns>The attribute name</returns>
	public static string? ExtractAttributeName(this NameSyntax? name, CancellationToken cancellationToken, out string[] genericTypeParams)
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
					genericTypeParams = genericNameSyntax.TypeArgumentList.Arguments.Select(argument => argument.ToString()).ToArray();
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