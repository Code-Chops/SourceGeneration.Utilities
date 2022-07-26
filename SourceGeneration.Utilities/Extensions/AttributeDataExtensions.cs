namespace CodeChops.SourceGeneration.Utilities.Extensions;

/// <summary>
/// Provides extensions on <see cref="AttributeData"/>.
/// </summary>
public static class AttributeDataExtensions
{
	/// <summary>
	/// Tries to get the arguments of the attribute.
	/// </summary>
	public static bool TryGetArguments(this AttributeData attributeData, out Dictionary<string, ITypeSymbol>? argumentInfoByNames)
	{
		var constructorParameters = attributeData.AttributeConstructor?.Parameters;
		if (constructorParameters is null) 
		{
			argumentInfoByNames = null;
			return false; 
		}

		// Start with an indexed list of names for mandatory arguments
		var argumentNames = constructorParameters.Value.Select(parameterSymbol => parameterSymbol.Name).ToList();

		// Combine the argument names by their constant (retrieved from the constructor arguments).
		var argumentNameAndTypePairs = attributeData.ConstructorArguments.Select((info, index) => (Name: argumentNames[index], Type: info.Type!));
		
		// Create a dictionary with the argument as key and a 
		argumentInfoByNames = argumentNameAndTypePairs.ToDictionary(argument => argument.Name, argument => argument.Type);
		
		return true;
	}
}
