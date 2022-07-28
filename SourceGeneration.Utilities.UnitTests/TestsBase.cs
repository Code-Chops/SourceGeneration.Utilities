namespace CodeChops.SourceGeneration.Utilities.UnitTests;

public abstract class TestsBase
{
	protected abstract SyntaxTree SyntaxTree { get; }
	
	protected SemanticModel SemanticModel { get; } 
	
	protected TestsBase()
	{
		var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
		var compilation = CSharpCompilation.Create(
			assemblyName: nameof(TypeSymbolTests),
			syntaxTrees: new[] { this.SyntaxTree }, 
			references: new[] { mscorlib });

		this.SemanticModel = compilation.GetSemanticModel(this.SyntaxTree);
	}
}