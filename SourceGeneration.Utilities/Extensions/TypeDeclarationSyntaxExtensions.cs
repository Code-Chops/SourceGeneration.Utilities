using System.Text;

namespace CodeChops.SourceGeneration.Utilities.Extensions;

public static class TypeDeclarationSyntaxExtensions
{
	public static string? GetClassGenericConstraints(this TypeDeclarationSyntax type)
	{
		if (type.ConstraintClauses.Count == 0) return null;
		
		var constraints = type.ConstraintClauses
			.Aggregate(new StringBuilder(), (s, c) => s.Append(c.ToFullString()))
			.ToString();

		return constraints;
	}

	public static string? GetUsings(this TypeDeclarationSyntax type)
	{
		var root = type.SyntaxTree.GetRoot();
		var usings = root.ChildNodes().SingleOrDefault(node => node is UsingDirectiveSyntax);
		return usings?.ToString();
	}
}