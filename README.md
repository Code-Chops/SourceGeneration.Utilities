# SourceGenerationUtilities

Utilities for easy implementation of source generators.
A big thanks to [TheArchitectDev](https://github.com/TheArchitectDev/) for providing many of these extensions.

## Extensions
### AttributeDataExtensions
 - TryGetArguments: Tries to get the arguments of the attribute.
 - GetArgumentOrDefault: Gets the value of the attribute argument, or the (provided) default value if the argument was not provided.
 - TryGetArgument: Tries to get the value of the attribute argument.

### NamespaceSymbolExtensions
 - IsInSystemNamespace**:  Returns whether the given `INamedTypeSymbol` is or resides in the `System` namespace.
 - HasFullName**: Returns whether the given `INamedTypeSymbol` has the given 'fullName'.

### NameSyntaxExtensions
- HasAttributeName: Checks if a name syntax has a specific attribute of provided 'expectedName'.
- ExtractAttributeName: Extracts the attribute name from the name syntax.

### StringBuilderExtensions
- TrimEnd: Trims the end of a `StringBuilder`.
- AppendLine: Appends the `StringBuilder` with the provided text retriever (`Func<string?>`) and adds a newline if the resulting text is not `null`.

### StringExtensions
- ToTitleCase: Returns the input with the first character converted to uppercase.
- NormalizeWhitespace**: Normalizes the whitespace for the given C# source code as much as possible.
- GetStableHashCode32**: Gets a stable hash code (`int`).
- GetStableHashCode64**: Gets a stable hash code (`ulong`).
- GetStableStringHashCode32**: Gets a stable `int` hash code as `string`.
- GetStableStringHashCode64**: Gets a stable `ulong` hash code as `string`.
- ToBase32Chars8**: Converts the given 8 `bytes` to 13 base32 `chars`.
- Write: Returns specific text when the provided value is not `null`.

### TypeDeclarationSyntaxExtensions
- GetClassGenericConstraints: Gets the generic constraints of a `TypeDeclarationSyntax`.
- GetUsings: Gets the `usings` of a `TypeDeclarationSyntax`.

### TypeSymbolExtensions
- IsType**: Returns whether the `ITypeSymbol` is of a specific type.
- IsOrInheritsClass**: Returns whether the `ITypeSymbol` is or inherits from a certain `class`, as determined by the given predicate.
- IsOrImplementsInterface**: Returns whether the `ITypeSymbol` is or implements a certain `interface`, as determined by the given predicate.
- HasSingleGenericTypeArgument**: Returns whether the <see cref="ITypeSymbol"/> is a constructed generic type with a single type argument matching the provided 'requiredTypeArgument'.
- IsNumeric: Returns whether the <see cref="ITypeSymbol"/> represents an numeric type, such as an `int` or `ulong`.
- IsGeneric**: Returns whether the <see cref="ITypeSymbol"/> is a generic type (with the given number of type parameters).
- IsNullable**: Returns whether the <see cref="ITypeSymbol"/> is a `Nullable<T>`.
- IsSelfEquatable**: Returns whether the given <see cref="ITypeSymbol"/> implements `IEquatable{T}` against itself.
- IsComparable**: Returns whether the <see cref="ITypeSymbol"/> implements any `IComparable` or <see cref="IComparable{T}"/> interface.
- IsEnumerable**: Returns whether the <see cref="ITypeSymbol"/> is or implements `System.Collections.IEnumerable`.
- HasEqualsOverride**: Returns whether the <see cref="ITypeSymbol"/> or a base type has an override of <see cref="Object.Equals(object)"/> more specific than `Object`'s implementation.
- HasAttribute**: Returns whether the <see cref="ITypeSymbol"/> is annotated with the specified attribute(s).
- HasConversionTo**: Returns whether the <see cref="ITypeSymbol"/> defines a conversion to the specified type.
- HasConversionFrom**: Returns whether the <see cref="ITypeSymbol"/> defines a conversion from the specified type.
- GetAvailableConversionsFromNatives**: Enumerates the native types (`string`, `int`, `bool`, `decimal`, `double`, `char`, etc.) from which the given <see cref="ITypeSymbol"/> is convertible.
- CreateStringExpression**: Returns the code for a string expression of the given <paramref name="memberName"/> of "this".
- CreateComparisonExpression**: Returns the code for a comparison expression on the given 'memberName' between "this" and "other".
- GetFullTypeNameWithoutGenericParameters: Converts names like `string` to `global::System.String` excluding generic parameter names (if used).
- GetFullTypeNameWithGenericParameters: Converts names like `string` to `global::System.String` including generic parameter names (if used).
- GetTypeNameWithGenericParameters: Gets the type name including generic parameters (without `namespace`).
- GetTypeKindName: Gets the name of the TypeKind. For example: a `record class` will be 'record class', an `interface` will be 'interface'.
- GetObjectDeclaration: Gets the declaration of a `class`, `record` or `interface`. E.g.: 'public abstract (partial) class Test'.

## Helpers

### FileNameHelpers
- GetFileName: Gets a unique source generation file name (takes namespaces and generic parameters into account).
  - Removes 'global::' from the start of the name if it exists.
  - Replaces invalid file name characters to '_'.
  - Removes the root namespace of the source assembly from the start file name.
  - Adds suffix '.g.cs' to the file name, if needed.

### NameHelpers
- GetNameWithoutGenerics: Gets the name without generic parameters.
- HasGenericParameter: Checks if a name contains a generic parameter.
- GetNameWithGenerics: Gets the name of a class including the generic types. E.g: `System.Collections.Generic.Dictionary'2` becomes `System.Collections.Generic.Dictionary&lt;System.String,System.Object&gt;`.

> ** Has been provided by [TheArchitectDev](https://github.com/TheArchitectDev/).

