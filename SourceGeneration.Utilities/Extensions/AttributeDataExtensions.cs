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
	
	/// <summary>
	/// Gets the value of the attribute argument, or the provided default value if the argument was not provided.
	/// </summary>
	/// <typeparam name="T">The type of the argument parameter.</typeparam>
	/// <returns>The attribute argument as <typeparamref name="T"/> or the default value if the argument is not provided.</returns>
	/// <exception cref="InvalidCastException">When the argument cannot be casted to <typeparamref name="T"/></exception>
	public static T GetArgumentOrDefault<T>(this AttributeData attribute, string parameterName, T defaultValue)
	{
		if (!attribute.TryGetArgument<T>(parameterName, out var argument)) 
			return defaultValue;

		return argument!;
	}
	
	/// <summary>
	/// Tries to get the value of the attribute argument.
	/// </summary>
	/// <typeparam name="T">The type of the argument parameter.</typeparam>
	/// <returns>True if argument is retrieved.</returns>
	/// <exception cref="InvalidCastException">When the argument cannot be casted to <typeparamref name="T"/></exception>
	public static bool TryGetArgument<T>(this AttributeData attribute, string parameterName, out T? argument)
	{
		if (!attribute.TryGetArguments(out var argumentConstantByNames) || !argumentConstantByNames!.TryGetValue(parameterName, out var typedConstant) || typedConstant.Value is null)
		{
			argument = default;
			return false;
		}

		if (typedConstant.Value is not T value)
		{
			if (typeof(T).IsEnum)
			{
				argument = (T)typedConstant.Value;
				return true;
			}
		        
			throw new InvalidCastException($"Unable to cast value ({typedConstant.Value}) of \"{parameterName}\" to {typeof(T).Name}, from attribute for {attribute.AttributeClass?.Name}.");
		}

		argument = value;
		return true;
	}
}