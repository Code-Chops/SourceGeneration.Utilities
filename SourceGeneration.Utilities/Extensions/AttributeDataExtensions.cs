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

		// Start with an indexed list of names for mandatory arguments
		var argumentNames = constructorParameters.Value.Select(parameterSymbol => parameterSymbol.Name).ToList();

		// Combine the argument names by their constant (retrieved from the constructor arguments).
		var argumentNameAndTypePairs = attributeData.ConstructorArguments.Select((info, index) => (Name: argumentNames[index], Type: info));
		
		// Create a dictionary with the argument as key and a 
		argumentConstantByNames = argumentNameAndTypePairs.ToDictionary(argument => argument.Name, argument => argument.Type);
		
		return true;
	}
	
	/// <summary>
	/// Tries to get the arguments of the attribute.
	/// </summary>
	public static bool TryGetArgumentsA(
		this AttributeData attributeData,
		out Dictionary<string, (string Type, object? Value)>? argumentByNames)
	{
		argumentByNames = new Dictionary<string, (string Type, object? Value)>(StringComparer.OrdinalIgnoreCase);
		var constructorParameters = attributeData.AttributeConstructor?.Parameters;

		if (constructorParameters is null) 
		{
			return false; 
		}

		// Start with an indexed list of names for mandatory arguments
		var argumentNames = constructorParameters.Value.Select(x => x.Name).ToList();

		var allArguments = attributeData.ConstructorArguments
			.Select((info, index) => new KeyValuePair<string, TypedConstant>(argumentNames[index], info));

		foreach (var argument in allArguments)
		{
			argumentByNames.Add(argument.Key, (argument.Value.Type!.GetFullTypeNameWithGenericParameters(), argument.Value.Value));
		}

		return true;
	}
}
