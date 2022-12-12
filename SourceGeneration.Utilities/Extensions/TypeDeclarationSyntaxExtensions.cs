using System.Text;

namespace CodeChops.SourceGeneration.Utilities.Extensions;

public static class TypeDeclarationSyntaxExtensions
{
	/// <summary>
	/// Gets the generic constraints of a TypeDeclarationSyntax.
	/// </summary>
	public static string? GetClassGenericConstraints(this TypeDeclarationSyntax type)
	{
		if (type.ConstraintClauses.Count == 0) return null;
		
		var constraints = type.ConstraintClauses
			.Aggregate(new StringBuilder(), (s, c) => s.Append(c.ToFullString()))
			.ToString();

		return constraints;
	}

	/// <summary>
	/// Gets the usings of a TypeDeclarationSyntax.
	/// </summary>
	public static IEnumerable<string> GetUsings(this TypeDeclarationSyntax type)
	{
		var root = type.SyntaxTree.GetRoot();
		
		var usings = root
			.ChildNodes()
			.Where(node => node is UsingDirectiveSyntax)
			.Select(u => u.ToString());
		
		return usings;
	}
}
