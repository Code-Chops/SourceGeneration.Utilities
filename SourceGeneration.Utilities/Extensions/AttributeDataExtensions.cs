namespace CodeChops.SourceGeneration.Utilities.Extensions;

/// <summary>
/// Provides extensions on <see cref="AttributeData"/>.
/// </summary>
public static class AttributeDataExtensions
{
	/// <summary>
	/// Tries to get the arguments of the attribute.
	/// </summary>
	public static bool TryGetArguments(this AttributeData attributeData, out Dictionary<string, TypedConstant>? argumentConstantByNames)
	{
		var constructorParameters = attributeData.AttributeConstructor?.Parameters;
		if (constructorParameters is null) 
		{
			argumentConstantByNames = null;
			return false; 
		}

		// Start with an indexed list of names for mandatory arguments.
		var argumentNames = constructorParameters.Value.Select(parameterSymbol => parameterSymbol.Name).ToList();

		// Combine the argument names by their constant (retrieved from the constructor arguments).
		var argumentNameAndTypePairs = attributeData.ConstructorArguments.Select((type, index) => (Name: argumentNames[index], Type: type));
		
		// Create a dictionary with the argument as key and argument constant by value.
		argumentConstantByNames = argumentNameAndTypePairs.ToDictionary(argument => argument.Name, argument => argument.Type, StringComparer.OrdinalIgnoreCase);
		
		return true;
	}
}